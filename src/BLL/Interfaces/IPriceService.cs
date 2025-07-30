using BLL.DTOs;
using BLL.DTOs.Price;

namespace BLL.Interfaces;

public interface IPriceService
{
    Task<PriceDto> GetHighestBid(string pair);
    
    Task<PriceDto> GetLowestAsk(string pair);
}