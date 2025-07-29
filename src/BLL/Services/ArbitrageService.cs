using BLL.DTOs;
using BLL.Extensions;
using BLL.Interfaces;

namespace BLL.Services;

public class ArbitrageService : IArbitrageService
{
    
    private readonly IEnumerable<IExchangeApiClient> _clients;

    public ArbitrageService(IEnumerable<IExchangeApiClient> clients)
    {
        _clients = clients;
    }
    
    public async Task<IEnumerable<ArbitrageDbDto>> GetQuotes(string pair)
    {
        var tasks = _clients.Select(async client =>
        {
            var rateDto = await client.GetTicker(pair);
            return new ArbitrageDbDto
            {
                Exchange = client.ExchangeName,
                Bid      = rateDto.Bid,
                Ask      = rateDto.Ask
            };
        });

        var results = await Task.WhenAll(tasks);
        return results.Where(x => x != null)!;
    }

    public async Task<ArbitrageQuickDto> GetOpportunityAsync(string pair)
    {
        var tasks = _clients.Select(async c =>
        {
            try
            {
                var t = await c.GetTicker(pair);
                return (Exchange: c.ExchangeName, t.Bid, t.Ask);
            }
            catch
            {
                return (Exchange: (string?)null, Bid: (decimal?)null, Ask: (decimal?)null);
            }
        });

        var all = await Task.WhenAll(tasks);

        var quotes = all
            .Where(x => x.Exchange is not null && x.Bid.HasValue && x.Ask.HasValue)
            .Select(x => (Exchange: x.Exchange!,
                Bid: x.Bid!.Value,
                Ask: x.Ask!.Value))
            .ToList();

        if (!quotes.Any())
            throw new InvalidOperationException($"No data for pair {pair}");

        var bestBuy = quotes.OrderBy(q => q.Ask).First();
        var bestSell = quotes.OrderByDescending(q => q.Bid).First();

        return new ArbitrageQuickDto()
        {
            Pair = pair,
            Buy  = new PriceDto() {
                Pair     = pair,
                Exchange = bestBuy.Exchange,
                Price    = bestBuy.Ask },
            Sell = new PriceDto() {
                Pair     = pair,
                Exchange = bestSell.Exchange,
                Price    = bestSell.Bid }
        };
    }
}