using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config.Mexc;
using DAL.Interfaces;

namespace BLL.Services;

public class MexcApiClient : IExchangeApiClient
{
    
    private readonly HttpClient _http;
    private const string _exchangeName = "MEXC";
    private readonly HashSet<string> _supportedSymbols;

    public MexcApiClient(HttpClient http, ICurrencyPairRepository pairRepo)
    {
        _http = http;
        
        var pairs = pairRepo.GetAllAsync().Result;
        
        _supportedSymbols = pairs
            .Select(p => p.FromCurrencyNavigation.Symbol + p.ToCurrencyNavigation.Symbol)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
    
    
    public async Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default)
    {
        
        var raw = await _http.GetFromJsonAsync<List<MexcTicker>>(
            "/api/v3/ticker/price", ct);

        if (raw == null)
            return Enumerable.Empty<CurrencyPairRateDto>();

        return raw
            .Where(t => _supportedSymbols.Contains(t.Symbol))
            .Select(t => new CurrencyPairRateDto {
                PairSymbol   = $"{t.Symbol.Substring(0, t.Symbol.Length-4)}/{t.Symbol.Substring(t.Symbol.Length-4)}",
                Rate         = decimal.Parse(t.Price),
                ExchangeName = _exchangeName
            });
        
    }
}