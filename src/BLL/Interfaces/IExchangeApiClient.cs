using BLL.DTOs;

namespace BLL.Interfaces;

public interface IExchangeApiClient
{
    
    Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default);
    
}