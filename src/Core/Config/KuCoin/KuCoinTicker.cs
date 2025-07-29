using System.Text.Json.Serialization;

namespace Core.Config.KuCoin;

public class KuCoinTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("last")]
    public string Last { get; set; } = null!;
}