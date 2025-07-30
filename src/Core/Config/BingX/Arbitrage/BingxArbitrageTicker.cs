using System.Text.Json.Serialization;

namespace Core.Config.BingX.Arbitrage;

public class BingxArbitrageTicker
{

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("bidPrice")]
    public string BidPrice { get; set; } = null!;


    [JsonPropertyName("askPrice")]
    public string AskPrice { get; set; } = null!;

}