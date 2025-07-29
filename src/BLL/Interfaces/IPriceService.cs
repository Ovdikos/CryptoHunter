using BLL.DTOs;

namespace BLL.Interfaces;

public interface IPriceService
{
    Task<PriceDto> GetHighestBidAsync(string pair);
    
    Task<PriceDto> GetLowestAskAsync(string pair);
}