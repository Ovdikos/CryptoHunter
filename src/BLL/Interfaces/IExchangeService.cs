using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;

namespace BLL.Interfaces;

public interface IExchangeService
{
    
    string ExchangeName { get; }
    
    Task<TickerResponseDto> GetTicker(string pair, CancellationToken ct = default);
    
    Task<Exchange24hDto> Get24hStats(string pair, CancellationToken ct = default);
    
}