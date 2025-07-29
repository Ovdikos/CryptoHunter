using System.Net;
using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config.Coinbase;
using DAL.Interfaces;

namespace BLL.Services;

public class CoinbaseApiClient : IExchangeApiClient
{
    
    private readonly HttpClient _http;
    private const string _exchangeName = "Coinbase";
    private readonly HashSet<string> _supportedSymbols;

    public CoinbaseApiClient(HttpClient http, ICurrencyPairRepository pairRepo)
    {
        _http = http;

        _supportedSymbols = pairRepo.GetAllAsync().Result
            .Select(p => p.FromCurrencyNavigation.Symbol 
                         + "-" 
                         + p.ToCurrencyNavigation.Symbol)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }


    public async Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default)
    {

        var result = new List<CurrencyPairRateDto>();

        foreach (var pair in _supportedSymbols)
        {
            var parts = pair.Split('-', 2);
            var baseCur = parts[0];
            var quoteCur = parts[1];

            var productId = $"{baseCur}-{quoteCur}";

            try
            {
                var resp = await _http.GetFromJsonAsync<CoinbaseResponse>(
                    $"/v2/prices/{productId}/spot", ct);

                if (resp?.Data != null)
                {
                    result.Add(new CurrencyPairRateDto
                    {
                        PairSymbol = $"{baseCur}/{quoteCur}",
                        Rate = decimal.Parse(resp.Data.Amount),
                        ExchangeName = _exchangeName
                    });
                }
            }
            catch (HttpRequestException e) when (
                e.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
            }
        }

        return result;
    }
}