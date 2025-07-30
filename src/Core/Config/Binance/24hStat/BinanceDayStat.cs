namespace Core.Config.Binance._24hStat;

public class BinanceDayStat
{
    
    public string openPrice         { get; set; } = null!;
    public string highPrice         { get; set; } = null!;
    public string lowPrice          { get; set; } = null!;
    public string lastPrice         { get; set; } = null!;
    public string volume            { get; set; } = null!;
    public string priceChangePercent { get; set; } = null!;
    public string weightedAvgPrice  { get; set; } = null!;
}