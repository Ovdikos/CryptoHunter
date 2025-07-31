using System.Net;
using System.Net.Http.Json;
using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Core.Config.Coinbase;
using Core.Config.Coinbase._24hStat;
using Core.Config.Coinbase.Arbitrage;
using DAL.Interfaces;

namespace BLL.Services;

public class CoinbaseService : IExchangeService
{
    
    private readonly HttpClient _http;
    private const string _exchangeName = "Coinbase";
    private readonly HashSet<string> _supportedSymbols;

    public CoinbaseService(HttpClient http, ICurrencyPairRepository pairRepo)
    {
        _http = http;
        
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("CryptoHunter/1.0");

        _supportedSymbols = pairRepo.GetAllAsync().Result
            .Select(p => p.FromCurrencyNavigation.Symbol 
                         + "-" 
                         + p.ToCurrencyNavigation.Symbol)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
    
    public string ExchangeName => _exchangeName;

    public async Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default)
    {
        var symbol = $"{pair[..^4]}-USDT";
        
        var raw = await _http.GetFromJsonAsync<CoinbaseArbitrageTicker>(
            $"/products/{symbol}/ticker", ct);
        
        if (raw is null)
            throw new InvalidOperationException($"Coinbase no data for {symbol}");
                
        return new TickerResponseDto
        {
            Symbol = pair,
            Bid    = decimal.Parse(raw.Bid),
            Ask    = decimal.Parse(raw.Ask)
        };
    }

    public async Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default)
    {
        var productId = $"{pair[..^4]}-{pair[^4..]}";

        var raw = await _http.GetFromJsonAsync<CoinbaseDayStatResponse>(
            $"/products/{productId}/stats", ct);
        if (raw is null)
            throw new InvalidOperationException($"Coinbase no 24h data for {pair}");

        var open   = decimal.Parse(raw.Open);
        var high   = decimal.Parse(raw.High);
        var low    = decimal.Parse(raw.Low);
        var close  = decimal.Parse(raw.Last);
        var vol    = decimal.Parse(raw.Volume);

        var pctChange   = open != 0 ? (close - open) / open * 100 : 0m;
        var weightedAvg = (open + high + low + close) / 4;

        return new Exchange24hDto
        {
            Exchange         = _exchangeName,
            Pair             = pair,
            Open             = open,
            High             = high,
            Low              = low,
            Close            = close,
            Volume           = vol,
            PriceChangePct   = pctChange,
            WeightedAvgPrice = weightedAvg
        };
    }
}