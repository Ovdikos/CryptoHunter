using BLL.DTOs;
using BLL.DTOs._24hStat;
using BLL.DTOs.PriceChangeIn24h;

namespace BLL.Interfaces;

public interface IMarketService
{
    Task<MarketSummaryDto> Get24hSummary(string pair);
    Task<IReadOnlyList<TopPairDto>> GetTopPairs(
        string type,
        int limit, 
        CancellationToken ct = default);
}