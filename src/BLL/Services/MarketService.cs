using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.Interfaces;

namespace BLL.Services;

public class MarketService : IMarketService
{
    private readonly IEnumerable<IExchangeApiClient> _clients;
    public MarketService(IEnumerable<IExchangeApiClient> clients)
        => _clients = clients;

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
}