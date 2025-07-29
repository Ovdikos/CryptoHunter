using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _svc;
        public PriceController(IPriceService svc) => _svc = svc;

        [HttpGet("maxbid/{coin}")]
        public async Task<ActionResult<PriceDto>> GetMaxBid(string coin)
        {
            var pair = coin.ToUpper() + "USDT";
            try
            {
                var result = await _svc.GetHighestBidAsync(pair);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"No data for pair {pair}");
            }
        }
        
        [HttpGet("minbid/{coin}")]
        public async Task<ActionResult<PriceDto>> GetMinBid(string coin)
        {
            var pair = coin.ToUpper() + "USDT";
            try
            {
                var result = await _svc.GetLowestAskAsync(pair);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"No data for pair {pair}");
            }
        }
    }
}