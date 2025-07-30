using BLL.DTOs;
using BLL.DTOs._24hStat;

namespace BLL.Interfaces;

public interface IMarketService
{
    Task<MarketSummaryDto> Get24hSummary(string pair);
}