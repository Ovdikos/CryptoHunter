using System.Text.Json.Serialization;

namespace Core.Config.Bybit;

public class BybitTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("lastPrice")]
    public string LastPrice { get; set; } = null!;
}