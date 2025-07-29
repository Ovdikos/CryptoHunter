using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArbitrageController : ControllerBase
    {
        private readonly IArbitrageService _arb;

        public ArbitrageController(IArbitrageService arb)
        {
            _arb = arb;
        }

        [HttpGet("database/{pair}")]
        public async Task<IActionResult> Get(string pair)
        {
            var quotes = (await _arb.GetQuotes(pair))
                .ToList();

            if (!quotes.Any())
                return NotFound($"No quotes for pair '{pair}'");

            var bestBuy  = quotes.OrderBy(q => q.Ask).First();
            var bestSell = quotes.OrderByDescending(q => q.Bid).First();
            var absolute = bestSell.Bid - bestBuy.Ask;
            var percent  = absolute / bestBuy.Ask * 100;

            return Ok(new
            {
                pair,
                bestBuy    = new { bestBuy.Exchange,  price = bestBuy.Ask  },
                bestSell   = new { bestSell.Exchange, price = bestSell.Bid  },
                spread     = new { absolute, percent },
                allQuotes  = quotes
            });
        }
        
        
        [HttpGet("opportunity/{coin}")]
        public async Task<ActionResult<ArbitrageQuickDto>> GetOp(string coin)
        {
            var pair = coin.ToUpper() + "USDT";
            try
            {
                var dto = await _arb.GetOpportunityAsync(pair);
                return Ok(dto);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"No data found for pair {pair}");
            }
        }
        
    }
}