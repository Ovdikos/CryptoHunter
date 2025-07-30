using System.Text.Json.Serialization;

namespace Core.Config.Mexc.Arbitrage;

public class MexcArbitrageTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("bidPrice")]
    public string BidPrice { get; set; } = null!;

    [JsonPropertyName("askPrice")]
    public string AskPrice { get; set; } = null!;
}