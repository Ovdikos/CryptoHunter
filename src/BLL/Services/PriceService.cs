using BLL.DTOs;
using BLL.DTOs.Price;
using BLL.Interfaces;

public class PriceService : IPriceService
{
    private readonly IEnumerable<IExchangeApiClient> _clients;

    public PriceService(IEnumerable<IExchangeApiClient> clients)
        => _clients = clients;

    public async Task<PriceDto> GetHighestBid(string pair)
    {
        var quotes = await CollectQuotesAsync(pair);
        var best = quotes.OrderByDescending(q => q.Price).First();
        return new PriceDto {
            Pair     = pair,
            Exchange = best.Exchange,
            Price    = best.Price
        };
    }

    public async Task<PriceDto> GetLowestAsk(string pair)
    {
        var quotes = await CollectQuotesAsync(pair);
        var best = quotes.OrderBy(q => q.Price).First();
        return new PriceDto {
            Pair     = pair,
            Exchange = best.Exchange,
            Price    = best.Price
        };
    }

    private async Task<List<(string Exchange, decimal Price)>> CollectQuotesAsync(string pair)
    {
        var tasks = _clients.Select(async c =>
        {
            try
            {
                var t = await c.GetTicker(pair);
                return (Exchange: c.ExchangeName, Price: (decimal?)t.Ask);
            }
            catch
            {
                return (Exchange: (string?)null, Price: (decimal?)null);
            }
        });
        
        var all = await Task.WhenAll(tasks);

        var filtered = all
            .Where(x => x.Exchange is not null && x.Price.HasValue)
            .Select(x => (x.Exchange!, x.Price!.Value))
            .ToList();

        return filtered;
    }

}