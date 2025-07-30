using BLL.DTOs;
using BLL.DTOs.Arbitrage;

namespace BLL.Interfaces;

public interface IArbitrageService
{
    Task<IEnumerable<ArbitrageDbDto>> GetQuotes(string pair);

    Task<ArbitrageQuickDto> GetOpportunity(string pair);
}