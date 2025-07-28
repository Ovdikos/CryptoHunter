using DAL.Entities;

namespace DAL.Interfaces;

public interface ICurrencyPairRepository
{
    Task<IEnumerable<CurrencyPair>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CurrencyPair?> GetBySymbolsAsync(
        string fromCurrencySymbol,
        string toCurrencySymbol,
        CancellationToken cancellationToken = default
    );
}