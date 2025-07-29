using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config;
using Core.Config.Bybit;
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


    public async Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default)
    {
        var symbolsCsv = string.Join(',', _supportedSymbols);

        var resp = await _http.GetFromJsonAsync<BybitResponse>(
            $"/v5/market/tickers?category=spot&symbol={symbolsCsv}", ct);

        var list = resp?.Result?.List;
        if (list == null || list.Count == 0)
            return Enumerable.Empty<CurrencyPairRateDto>();

        return list
            .Select(t => new CurrencyPairRateDto
            {
                PairSymbol   = $"{t.Symbol.Substring(0,3)}/{t.Symbol.Substring(3)}",
                Rate         = decimal.Parse(t.LastPrice),
                ExchangeName = _exchangeName
            });
    }
}