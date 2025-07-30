namespace BLL.DTOs.Arbitrage;

public class TickerResponseDto
{
    public string Symbol { get; set; } = null!;

    public decimal Bid { get; set; }

    public decimal Ask { get; set; }
}