using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

[ApiController]
[Route("api/[controller]")]
public class MainController : ControllerBase
{
    private readonly IEnumerable<IExchangeApiClient> _clients;

    public MainController(IEnumerable<IExchangeApiClient> clients)
    {
        _clients = clients;
    }

    [HttpGet("binance/ena")]
    public async Task<IActionResult> GetBinanceEna(CancellationToken ct)
    {
        var binance = _clients.First(c => c is BinanceApiClient);
        var r = (await binance.GetAllPairRates(ct))
            .FirstOrDefault(r => r.PairSymbol == "ENA/USDT");
        return r != null ? Ok(r) : NotFound();
    }

    [HttpGet("bybit/ena")]
    public async Task<IActionResult> GetBybitEna(CancellationToken ct)
    {
        var bybit = _clients.First(c => c is BybitApiClient);
        var r = (await bybit.GetAllPairRates(ct))
            .FirstOrDefault(r => r.PairSymbol == "ENA/USDT");
        return r != null ? Ok(r) : NotFound();
    }
    
    [HttpGet("bingx/ena")]
    public async Task<IActionResult> GetBingxEna(CancellationToken ct)
    {
        var bingx = _clients.First(c => c is BingxApiClient);
        var rate = (await bingx.GetAllPairRates(ct))
            .FirstOrDefault(r => r.PairSymbol == "ENA/USDT");
        return rate != null ? Ok(rate) : NotFound();
    }
    
    [HttpGet("mexc/ena")]
    public async Task<IActionResult> GetMexcEna(CancellationToken ct)
    {
        var mexcClient = _clients.First(c => c is MexcApiClient);
        var rate = (await mexcClient.GetAllPairRates(ct))
            .FirstOrDefault(r => r.PairSymbol == "ENA/USDT");

        return rate != null
            ? Ok(rate)
            : NotFound("ENA/USDT not found on MEXC");
    }
    
    [HttpGet("coinbase/ena")]
    public async Task<IActionResult> GetCoinbaseEna(CancellationToken ct)
    {
        var client = _clients
            .First(c => c.GetType() == typeof(CoinbaseApiClient));

        var rate = (await client.GetAllPairRates(ct))
            .FirstOrDefault(r => r.PairSymbol == "ENA/USDT");

        return rate != null 
            ? Ok(rate) 
            : NotFound("ENA/USDT not found on Coinbase");
    }
    
    [HttpGet("kucoin/ena")]
    public async Task<IActionResult> GetKuCoinEna(CancellationToken ct)
    {
        var client = _clients
            .OfType<KuCoinApiClient>()
            .First();

        var rate = (await client.GetAllPairRates(ct))
            .FirstOrDefault(r => r.PairSymbol == "ENA/USDT");

        return rate is not null
            ? Ok(rate)
            : NotFound("ENA/USDT not found on KuCoin");
    }

    [HttpGet("ena/all")]
    public async Task<IActionResult> GetEnaFromAllExchanges(CancellationToken ct)
    {
        var tasks = _clients
            .Select(c => c.GetAllPairRates(ct));
        var results = await Task.WhenAll(tasks);

        var enaRates = results
            .SelectMany(r => r)
            .Where(r => r.PairSymbol == "ENA/USDT") 
            .ToList();

        if (!enaRates.Any())
            return NotFound("ENA/USDT not found on any exchange");

        return Ok(enaRates);
    }
}