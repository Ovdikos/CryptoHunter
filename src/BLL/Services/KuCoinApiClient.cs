using System.Net.Http.Json;
using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Core.Config;
using Core.Config.KuCoin;
using Core.Config.KuCoin._24hStat;
using DAL.Interfaces;

namespace BLL.Services;

public class KuCoinApiClient : IExchangeApiClient
{
    
    private readonly HttpClient _http;
    private readonly string _exchangeName = "KuCoin";
    private readonly HashSet<string> _supportedSymbols;
    
    
    public KuCoinApiClient(HttpClient http, IEnumerable<ExchangeConfig> configs, ICurrencyPairRepository pairRepo)
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
        var symbol = $"{pair[..^4]}-USDT";
        var resp = await _http.GetFromJsonAsync<KuCoinArbitrageResponse>(
            $"/api/v1/market/orderbook/level1?symbol={symbol}", ct);

        if (resp?.Data is null)
            throw new InvalidOperationException($"KuCoin no data for {symbol}");

        var raw = resp.Data;
        return new TickerResponseDto
        {
            Symbol = pair,
            Bid    = decimal.Parse(raw.BestBid),
            Ask    = decimal.Parse(raw.BestAsk)
        };
    }

    public async Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default)
    {
        var symbol = $"{pair[..^4]}-{pair[^4..]}";

        var resp = await _http.GetFromJsonAsync<KuCoinDayStatResponse>(
            $"/api/v1/market/stats?symbol={symbol}", ct);

        if (resp?.Data == null)
            throw new InvalidOperationException($"KuCoin no 24h data for {pair}");

        var d = resp.Data;

        var last         = decimal.Parse(d.Last);
        var changePrice  = decimal.Parse(d.ChangePrice);
        var open         = last - changePrice;
        var high         = decimal.Parse(d.High);
        var low          = decimal.Parse(d.Low);
        var volume       = decimal.Parse(d.Volume);
        var changePct    = decimal.Parse(d.ChangeRate) * 100m;
        var weightedAvg  = decimal.Parse(d.AveragePrice);

        return new Exchange24hDto
        {
            Exchange         = _exchangeName,
            Pair             = pair,
            Open             = open,
            High             = high,
            Low              = low,
            Close            = last,
            Volume           = volume,
            PriceChangePct   = changePct,
            WeightedAvgPrice = weightedAvg
        };
    }
}