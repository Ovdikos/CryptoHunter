using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMarketService _svc;
    public MarketController(IMarketService svc) => _svc = svc;

    [HttpGet("24h/{pair}")]
    public async Task<ActionResult<MarketSummaryDto>> Get24h(string pair)
    {
        try
        {
            var dto = await _svc.Get24hSummary(pair.ToUpper());
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
}