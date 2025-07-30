using BLL.DTOs._24hStat;
using BLL.DTOs.Converter;
using BLL.DTOs.PriceChangeIn24h;
using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services;

public class MarketService : IMarketService
{
    private readonly IEnumerable<IExchangeApiClient> _clients;
    private readonly ICurrencyPairRepository _pairRepo;
    public MarketService(IEnumerable<IExchangeApiClient> clients, ICurrencyPairRepository pairRepo)
    {
        _clients = clients;
        _pairRepo = pairRepo;
    }

    public async Task<MarketSummaryDto> Get24hSummary(string pair)
    {
        var tasks = _clients.Select(async c =>
        {
            try
            {
                var stats = await c.Get24hStats(pair);
                return stats;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[{c.ExchangeName}] 24hStats failed: {ex.Message}");
                return null;
            }
        });

        var results = (await Task.WhenAll(tasks))
            .Where(x => x != null)!
            .ToList()!;

        if (!results.Any())
            throw new InvalidOperationException($"No 24h data for pair {pair}");

        var totalVolume = results.Sum(x => x.Volume);
        var weightedOpen = results.Sum(x => x.Open * x.Volume) / totalVolume;
        var aggHigh      = results.Max(x => x.High);
        var aggLow       = results.Min(x => x.Low);
        var weightedClose= results.Sum(x => x.Close * x.Volume) / totalVolume;
        var weightedAvg  = results.Sum(x => x.WeightedAvgPrice * x.Volume) / totalVolume;
        var changePct    = (weightedClose - weightedOpen) / weightedOpen * 100;

        var aggregated = new Exchange24hDto
        {
            Exchange         = "ALL",
            Pair             = pair,
            Open             = weightedOpen,
            High             = aggHigh,
            Low              = aggLow,
            Close            = weightedClose,
            Volume           = totalVolume,
            PriceChangePct   = changePct,
            WeightedAvgPrice = weightedAvg
        };

        return new MarketSummaryDto
        {
            Pair        = pair,
            PerExchange = results,
            Aggregated  = aggregated
        };
    }

    public async Task<IReadOnlyList<TopPairDto>> GetTopPairs(string type, int limit, CancellationToken ct = default)
    {
        var pairs = await _pairRepo.GetAllAsync();

        var tasks = pairs.Select(async p =>
        {
            try
            {
                var summary = await Get24hSummary(p.FromCurrencyNavigation.Symbol
                                                  + p.ToCurrencyNavigation.Symbol);
                return new TopPairDto {
                    Pair            = p.FromCurrencyNavigation.Symbol
                                      + p.ToCurrencyNavigation.Symbol,
                    PriceChangePct  = summary.Aggregated.PriceChangePct,
                    Volume          = summary.Aggregated.Volume
                };
            }
            catch
            {
                return null;
            }
        });

        var all = (await Task.WhenAll(tasks))
            .Where(x => x != null)!
            .ToList()!;

        var sorted = type.ToLower() switch
        {
            "gainers" => all.OrderByDescending(x => x.PriceChangePct),
            "losers"  => all.OrderBy(x => x.PriceChangePct),
            _         => throw new ArgumentException("type must be 'gainers' or 'losers'")
        };
        return sorted.Take(limit).ToList();
    }

    public async Task<ConversionDto> Convert(string fromPair, string toPair, decimal amount)
    {
        var bidTasks = _clients.Select(async c =>
        {
            try
            {
                var t = await c.GetTicker(fromPair);
                return (exchange: c.ExchangeName, bid: (decimal?)t.Bid);
            }
            catch
            {
                return (exchange: c.ExchangeName, bid: (decimal?)null);
            }
        });
        var bidResults = await Task.WhenAll(bidTasks);

        var validBids = bidResults.Where(x => x.bid.HasValue);
        if (!validBids.Any())
            throw new InvalidOperationException($"No bids for {fromPair}");

        var bestBidEntry = validBids
            .OrderByDescending(x => x.bid!.Value)
            .First();

        var askTasks = _clients.Select(async c =>
        {
            try
            {
                var t = await c.GetTicker(toPair);
                return (exchange: c.ExchangeName, ask: (decimal?)t.Ask);
            }
            catch
            {
                return (exchange: c.ExchangeName, ask: (decimal?)null);
            }
        });
        var askResults = await Task.WhenAll(askTasks);

        var validAsks = askResults.Where(x => x.ask.HasValue);
        if (!validAsks.Any())
            throw new InvalidOperationException($"No asks for {toPair}");

        var bestAskEntry = validAsks
            .OrderBy(x => x.ask!.Value)
            .First();

        var usdtAmount = amount * bestBidEntry.bid!.Value;
        var toAmount   = usdtAmount / bestAskEntry.ask!.Value;

        return new ConversionDto
        {
            FromPair    = fromPair,
            ToPair      = toPair,
            FromAmount  = amount,
            UsdtAmount  = usdtAmount,
            ToAmount    = toAmount,
            BestBid     = bestBidEntry.bid.Value,
            BidExchange = bestBidEntry.exchange,
            BestAsk     = bestAskEntry.ask.Value,
            AskExchange = bestAskEntry.exchange
        };
    }
}