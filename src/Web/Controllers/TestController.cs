using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class TestController : ControllerBase
{
    private readonly IExchangeApiClient _binance;

    public TestController(IEnumerable<IExchangeApiClient> clients)
    {
        _binance = clients.First(c => c.GetType() == typeof(BinanceApiClient));
    }

    [HttpGet("binance/ena")]
    public async Task<IActionResult> GetEnaUsdt(CancellationToken ct)
    {
        var rates = await _binance.GetAllPairRates(ct);
        var ena = rates.FirstOrDefault(r => r.PairSymbol == "ENA/USDT");
        return ena is not null
            ? Ok(ena)
            : NotFound("ENA/USDT not supported or not found");
    }
}