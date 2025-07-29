namespace Core.Config.Binance;

public class BinanceBookTicker
{
    public string symbol   { get; set; } = null!;
    public string bidPrice { get; set; } = null!;
    public string askPrice { get; set; } = null!;
}