using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CurrencyPairRepository : ICurrencyPairRepository
{
    private readonly CryptoContext _ctx;

    public CurrencyPairRepository(CryptoContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<CurrencyPair>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _ctx.CurrencyPairs
            .Include(cp => cp.FromCurrencyNavigation)
            .Include(cp => cp.ToCurrencyNavigation)
            .ToListAsync(cancellationToken);
    }
    
}