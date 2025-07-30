using BLL.DTOs.Price;

namespace BLL.DTOs.Arbitrage;

public class ArbitrageQuickDto
{
    public string Pair     { get; set; } = null!;

    public PriceDto Buy  { get; set; } = null!;
    public PriceDto Sell { get; set; } = null!;
}