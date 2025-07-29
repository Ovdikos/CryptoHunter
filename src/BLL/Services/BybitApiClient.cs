using System.Net;
using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config;
using Core.Config.Bybit;
using Core.Config.Bybit.Arbitrage;
using Core.Config.Bybit.Coin;
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


    public async Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default)
    {
        var list = new List<CurrencyPairRateDto>();

        foreach (var sym in _supportedSymbols)
        {
            try
            {
                var resp = await _http.GetFromJsonAsync<
                    BybitResponse<BybitTickerListResult>>(
                    $"/v5/market/tickers?category=spot&symbol={sym}", ct);

                var item = resp?.Result?.List?.FirstOrDefault();
                if (item == null)
                    continue;

                list.Add(new CurrencyPairRateDto
                {
                    PairSymbol   = sym.Insert(sym.Length - 4, "/"),  
                    Rate         = decimal.Parse(item.LastPrice),
                    ExchangeName = _exchangeName
                });
            }
            catch (HttpRequestException e) when (
                e.StatusCode == HttpStatusCode.NotFound ||
                e.StatusCode == HttpStatusCode.BadRequest)
            {
            }
        }

        return list;
    }

    public async Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default)
    {
        var url = $"/v5/market/tickers?category=spot&symbol={pair}";
        var resp = await _http.GetFromJsonAsync<
            BybitResponse<BybitTickerListResult>>(url, ct);

        var item = resp?.Result?.List?.FirstOrDefault()
                   ?? throw new InvalidOperationException($"Bybit no data for {pair}");

        return new TickerResponseDto
        {
            Symbol = item.Symbol,
            Bid    = decimal.Parse(item.Bid1Price),
            Ask    = decimal.Parse(item.Ask1Price)
        };
    }
}