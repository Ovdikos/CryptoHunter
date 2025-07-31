using System.Net.Http.Json;
using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Core.Config.BingX;
using Core.Config.BingX._24hStat;
using Core.Config.BingX.Arbitrage;
using DAL.Interfaces;

namespace BLL.Services;

public class BingxService : IExchangeService
{
    private readonly HttpClient _http;
    private readonly string _exchangeName = "BingX";
    private readonly HashSet<string> _supportedSymbols;

    public BingxService(HttpClient http, ICurrencyPairRepository pairRepo)
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
        var symbol = $"{pair[..^4]}-USDT";
        var resp = await _http.GetFromJsonAsync<BingxArbitrageResponse>(
            $"/openApi/spot/v1/ticker/bookTicker?symbol={symbol}", ct);
        
        var raw = resp?.Data?.FirstOrDefault()
                  ?? throw new InvalidOperationException($"BingX no data for {pair}");

        return new TickerResponseDto
        {
            Symbol = raw.Symbol.Replace("-", ""),
            Bid    = decimal.Parse(raw.BidPrice),
            Ask    = decimal.Parse(raw.AskPrice)
        };
    }

    public async Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default)
    {
        
        var symbol = $"{pair[..^4]}-{pair[^4..]}";    
        var resp = await _http.GetFromJsonAsync<BingxDayStat>(
            $"/openApi/market/his/v1/kline?symbol={symbol}&interval=1d&limit=1", ct);

        if (resp?.Data == null || resp.Data.Count == 0)
            throw new InvalidOperationException($"BingX no 24h kline data for {pair}");

        var candle = resp.Data[0];
        var open        = candle[1];
        var high        = candle[2];
        var low         = candle[3];
        var close       = candle[4];
        var baseVolume  = candle[5];
        var quoteVolume = candle[7];

        var pctChange   = open != 0 ? (close - open) / open * 100 : 0m;
        var weightedAvg = (open + high + low + close) / 4;

        return new Exchange24hDto
        {
            Exchange         = ExchangeName,
            Pair             = pair,
            Open             = open,
            High             = high,
            Low              = low,
            Close            = close,
            Volume           = baseVolume,
            PriceChangePct   = pctChange,
            WeightedAvgPrice = weightedAvg
        };
        
    }
}