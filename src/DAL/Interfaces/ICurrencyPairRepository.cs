using DAL.Entities;

namespace DAL.Interfaces;

public interface ICurrencyPairRepository
{
    Task<IEnumerable<CurrencyPair>> GetAllAsync(CancellationToken cancellationToken = default);
}