namespace Core.Config.Binance.Arbitrage;

public class BinanceArbitrage
{
    public string symbol   { get; set; } = null!;
    public string bidPrice { get; set; } = null!;
    public string askPrice { get; set; } = null!;
}