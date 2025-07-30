using System.Net;
using System.Net.Http.Json;
using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Core.Config;
using Core.Config.Bybit;
using Core.Config.Bybit._24hStat;
using Core.Config.Bybit.Arbitrage;
using DAL.Interfaces;

namespace BLL.Services;

public class BybitApiClient : IExchangeApiClient
{
    
    private readonly HttpClient _http;
    private readonly string _exchangeName = "Bybit";
    private readonly HashSet<string> _supportedSymbols;


    public BybitApiClient(HttpClient http, IEnumerable<ExchangeConfig> configs, ICurrencyPairRepository pairRepo)
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
        var url = $"/v5/market/tickers?category=spot&symbol={pair}";
        var resp = await _http.GetFromJsonAsync<
            BybitArbitrageResponse<BybitArbitrageResult>>(url, ct);

        var item = resp?.Result?.List?.FirstOrDefault()
                   ?? throw new InvalidOperationException($"Bybit no data for {pair}");

        return new TickerResponseDto
        {
            Symbol = item.Symbol,
            Bid    = decimal.Parse(item.Bid1Price),
            Ask    = decimal.Parse(item.Ask1Price)
        };
    }

    public async Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default)
    {
        var url = $"/v5/market/tickers?category=spot&symbol={pair}";

        var resp = await _http.GetFromJsonAsync<BybitDayStatResponse>(url, ct);

        if (resp is null 
            || resp.Result?.List == null 
            || resp.Result.List.Count == 0)
        {
            throw new InvalidOperationException($"Bybit no 24h data for {pair}");
        }

        var t = resp.Result.List[0];

        var open  = decimal.Parse(t.PrevPrice24h);        
        var close = decimal.Parse(t.LastPrice);            
        var high  = decimal.Parse(t.HighPrice24h);
        var low   = decimal.Parse(t.LowPrice24h);
        var vol   = decimal.Parse(t.Volume24h);
        var pct   = decimal.Parse(t.Price24hPcnt) * 100;   

        var weightedAvg = vol > 0 ? (open + high + low + close) / 4 : close;

        return new Exchange24hDto
        {
            Exchange         = ExchangeName,
            Pair             = pair,
            Open             = open,
            High             = high,
            Low              = low,
            Close            = close,
            Volume           = vol,
            PriceChangePct   = pct,
            WeightedAvgPrice = weightedAvg
        };
    }
}