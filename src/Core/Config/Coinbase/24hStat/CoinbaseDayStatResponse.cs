using System.Text.Json.Serialization;

namespace Core.Config.Coinbase._24hStat;

public class CoinbaseDayStatResponse
{
    [JsonPropertyName("open")]
    public string Open { get; set; } = null!;

    [JsonPropertyName("high")]
    public string High { get; set; } = null!;

    [JsonPropertyName("low")]
    public string Low { get; set; } = null!;

    [JsonPropertyName("last")]
    public string Last { get; set; } = null!;

    [JsonPropertyName("volume")]
    public string Volume { get; set; } = null!;
}