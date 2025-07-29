using BLL.DTOs;
using BLL.Interfaces;

namespace BLL.Extensions;

public static class ExchangeApiClient
{
    public static async Task<CurrencyPairRateDto> GetCurrencyPairRate(
        this IExchangeApiClient client,
        string pair,
        CancellationToken ct = default)
    {
        var all = await client.GetAllPairRates(ct);

        var rate = all
            .FirstOrDefault(r => 
                string.Equals(r.PairSymbol, pair, StringComparison.OrdinalIgnoreCase));

        if (rate is null)
            throw new InvalidOperationException(
                $"Pair '{pair}' not found on exchange '{client.ExchangeName}'");

        return rate;
    }

}