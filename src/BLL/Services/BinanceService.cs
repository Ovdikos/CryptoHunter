using System.Net.Http.Json;
using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Core.Config;
using Core.Config.Binance;
using Core.Config.Binance._24hStat;
using Core.Config.Binance.Arbitrage;
using DAL.Interfaces;

namespace BLL.Services;

public class BinanceService : IExchangeService
{
    
    
    private readonly HttpClient _http;
    private readonly string _exchangeName = "Binance";
    private readonly HashSet<string> _supportedSymbols;

    public BinanceService(HttpClient http, IEnumerable<ExchangeConfig> configs, ICurrencyPairRepository pairRepo)
    {
        _http = http;
        
        var pairsFromDb = pairRepo.GetAllAsync().Result; 
        
        _supportedSymbols = pairsFromDb
            .Select(p => $"{p.FromCurrencyNavigation.Symbol}{p.ToCurrencyNavigation.Symbol}")
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public string ExchangeName => _exchangeName;


    public async Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default)
    {
        var raw = await _http.GetFromJsonAsync<BinanceArbitrage>(
            $"/api/v3/ticker/bookTicker?symbol={pair}", ct);

        if (raw is null)
            throw new InvalidOperationException($"Binance returned no data for {pair}");

        return new TickerResponseDto
        {
            Symbol = raw.symbol,
            Bid    = decimal.Parse(raw.bidPrice),
            Ask    = decimal.Parse(raw.askPrice)
        };
    }

    public async Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default)
    {
        var raw = await _http.GetFromJsonAsync<BinanceDayStat>(
            $"/api/v3/ticker/24hr?symbol={pair}", ct);

        if (raw is null)
            throw new InvalidOperationException($"Binance no 24h data for {pair}");

        return new Exchange24hDto
        {
            Exchange          = ExchangeName,
            Pair              = pair,
            Open              = decimal.Parse(raw.openPrice),
            High              = decimal.Parse(raw.highPrice),
            Low               = decimal.Parse(raw.lowPrice),
            Close             = decimal.Parse(raw.lastPrice),
            Volume            = decimal.Parse(raw.volume),
            PriceChangePct    = decimal.Parse(raw.priceChangePercent),
            WeightedAvgPrice  = decimal.Parse(raw.weightedAvgPrice)
        };
    }
}