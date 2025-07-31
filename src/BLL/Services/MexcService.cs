using System.Net.Http.Json;
using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Core.Config.Mexc;
using Core.Config.Mexc._24hStat;
using Core.Config.Mexc.Arbitrage;
using DAL.Interfaces;

namespace BLL.Services;

public class MexcService : IExchangeService
{
    
    private readonly HttpClient _http;
    private const string _exchangeName = "MEXC";
    private readonly HashSet<string> _supportedSymbols;

    public MexcService(HttpClient http, ICurrencyPairRepository pairRepo)
    {
        _http = http;
        
        var pairs = pairRepo.GetAllAsync().Result;
        
        _supportedSymbols = pairs
            .Select(p => p.FromCurrencyNavigation.Symbol + p.ToCurrencyNavigation.Symbol)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
    
    public string ExchangeName => _exchangeName;
    

    public async Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default)
    {
        var symbol = $"{pair[..^4]}_USDT";
        var raw = await _http.GetFromJsonAsync<MexcArbitrageTicker>(
                      $"/api/v3/ticker/bookTicker?symbol={pair}", ct)
                  ?? throw new InvalidOperationException($"MEXC no data for {pair}");

        return new TickerResponseDto
        {
            Symbol = raw.Symbol.Replace("_", ""),
            Bid    = decimal.Parse(raw.BidPrice),
            Ask    = decimal.Parse(raw.AskPrice)
        };
    }

    public async Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default)
    {
        var resp = await _http.GetFromJsonAsync<MexcDayStatResponse>(
            $"/api/v3/ticker/24hr?symbol={pair}", ct);

        if (resp == null)
            throw new InvalidOperationException($"MEXC no 24h data for {pair}");

        var open        = decimal.Parse(resp.OpenPrice);
        var high        = decimal.Parse(resp.HighPrice);
        var low         = decimal.Parse(resp.LowPrice);
        var close       = decimal.Parse(resp.LastPrice);
        var volume      = decimal.Parse(resp.Volume);
        var quoteVolume = decimal.Parse(resp.QuoteVolume);
        var pctChange   = decimal.Parse(resp.PriceChangePercent);

        var weightedAvg = volume > 0 ? quoteVolume / volume : close;

        return new Exchange24hDto
        {
            Exchange         = _exchangeName,
            Pair             = pair,
            Open             = open,
            High             = high,
            Low              = low,
            Close            = close,
            Volume           = volume,
            PriceChangePct   = pctChange,
            WeightedAvgPrice = weightedAvg
        };
    }
}