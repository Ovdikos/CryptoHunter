using BLL.DTOs;

namespace BLL.Interfaces;

public interface IExchangeApiClient
{
    
    string ExchangeName { get; }
    
    Task<IEnumerable<CurrencyPairRateDto>> GetAllPairRates(CancellationToken ct = default);
    
    Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default);
    
}