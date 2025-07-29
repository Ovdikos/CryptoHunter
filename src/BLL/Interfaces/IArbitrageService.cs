using BLL.DTOs;

namespace BLL.Interfaces;

public interface IArbitrageService
{
    Task<IEnumerable<ArbitrageDbDto>> GetQuotes(string pair);

    Task<ArbitrageQuickDto> GetOpportunityAsync(string pair);
}