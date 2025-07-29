using System.Text.Json.Serialization;

namespace Core.Config.Mexc;

public class MexcBookTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("bidPrice")]
    public string BidPrice { get; set; } = null!;

    [JsonPropertyName("bidQty")]
    public string BidQty { get; set; } = null!;

    [JsonPropertyName("askPrice")]
    public string AskPrice { get; set; } = null!;

    [JsonPropertyName("askQty")]
    public string AskQty { get; set; } = null!;
}