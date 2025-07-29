using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config.BingX;
using DAL.Interfaces;

namespace BLL.Services;

public class BingxApiClient : IExchangeApiClient
{
    private readonly HttpClient _http;
    private readonly string _exchangeName = "BingX";
    private readonly HashSet<string> _supportedSymbols;

    public BingxApiClient(HttpClient http, ICurrencyPairRepository pairRepo)
    {
        _http = http;

        var pairs = pairRepo.GetAllAsync().Result;
        _supportedSymbols = pairs
            .Select(p => p.FromCurrencyNavigation.Symbol + p.ToCurrencyNavigation.Symbol)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default)
    {

        var list = new List<CurrencyPairRateDto>();

        foreach (var sym in _supportedSymbols)
        {
            var symbol = sym.Insert(sym.Length - 4, "-");

            var resp = await _http.GetFromJsonAsync<BingxResponse>(
                $"/openApi/swap/v2/quote/price?symbol={symbol}", ct);

            if (resp?.Data != null)
            {
                list.Add(new CurrencyPairRateDto
                {
                    PairSymbol = symbol.Replace("-", "/"),
                    Rate = decimal.Parse(resp.Data.Price),
                    ExchangeName = _exchangeName
                });

            }
        }
        return list;
    }
}