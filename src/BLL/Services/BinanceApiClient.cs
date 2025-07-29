using System.Net.Http.Json;
using BLL.DTOs;
using BLL.Interfaces;
using Core.Config;
using Core.Config.Binance;
using DAL.Interfaces;

namespace BLL.Services;

public class BinanceApiClient : IExchangeApiClient
{
    
    
    private readonly HttpClient _http;
    private readonly string _exchangeName = "Binance";
    private readonly HashSet<string> _supportedSymbols;

    public BinanceApiClient(HttpClient http, IEnumerable<ExchangeConfig> configs, ICurrencyPairRepository pairRepo)
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
        
        var raw = await _http.GetFromJsonAsync<List<BinanceTicker>>("/api/v3/ticker/price", ct);

        if (raw is null)
            return Enumerable.Empty<CurrencyPairRateDto>();

        return raw
            .Where(t => _supportedSymbols.Contains(t.Symbol))
            .Select(t => new CurrencyPairRateDto
            {
                PairSymbol   = $"{t.Symbol.Substring(0,3)}/{t.Symbol.Substring(3)}",
                Rate         = decimal.Parse(t.Price),
                ExchangeName = _exchangeName
            });
    }

    public async Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default)
    {
        var raw = await _http.GetFromJsonAsync<BinanceBookTicker>(
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
}