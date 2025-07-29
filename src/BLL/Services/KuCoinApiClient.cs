using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config;
using Core.Config.KuCoin;
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
    
    public async Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default)
    {
        
        var result = new List<CurrencyPairRateDto>();

        var resp = await _http.GetFromJsonAsync<KuCoinResponse>(
            "/api/v1/market/allTickers", ct);

        var tickers = resp?.Data?.Ticker;
        if (tickers == null) 
            return result;

        foreach (var t in tickers)
        {
            var raw = t.Symbol;
            var normalized = raw.Replace("-", "");

            if (!_supportedSymbols.Contains(normalized))
                continue;

            if (decimal.TryParse(t.Last, out var price))
            {
                result.Add(new CurrencyPairRateDto
                {
                    PairSymbol   = raw.Replace("-", "/"),
                    Rate         = price,
                    ExchangeName = _exchangeName
                });
            }
        }

        return result;
    }

    public async Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default)
    {
        var symbol = $"{pair[..^4]}-USDT";
        var resp = await _http.GetFromJsonAsync<KuCoinBookResponse>(
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
}