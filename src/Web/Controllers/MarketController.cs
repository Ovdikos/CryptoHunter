using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.Converter;
using BLL.DTOs.PriceChangeIn24h;
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
    [HttpGet("toppairs")]
    [ProducesResponseType(typeof(IEnumerable<TopPairDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTopPairs(
        [FromQuery] string type,
        [FromQuery] int limit = 10,
        CancellationToken ct = default)
    {
        if (limit <= 0)
            return BadRequest("Limit must be greater than zero.");

        type = type?.ToLowerInvariant() ?? "";
        if (type != "gainers" && type != "losers")
            return BadRequest("Type must be either 'gainers' or 'losers'.");

        var result = await _svc.GetTopPairs(type, limit, ct);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<ConversionDto>> Get(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] decimal amount)
    {
        if (string.IsNullOrWhiteSpace(from) ||
            string.IsNullOrWhiteSpace(to)   ||
            amount <= 0)
        {
            return BadRequest("Parameters 'from', 'to' and positive 'amount' are required.");
        }

        try
        {
            var result = await _svc.Convert(from, to, amount);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}