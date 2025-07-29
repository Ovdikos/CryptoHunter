using System.Text.Json.Serialization;

namespace Core.Config.BingX;

public class BingxBookTicker
{
    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = null!;

    [JsonPropertyName("time")]
    public long Time { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("bidPrice")]
    public string BidPrice { get; set; } = null!;

    [JsonPropertyName("bidVolume")]
    public string BidVolume { get; set; } = null!;

    [JsonPropertyName("askPrice")]
    public string AskPrice { get; set; } = null!;

    [JsonPropertyName("askVolume")]
    public string AskVolume { get; set; } = null!;
}