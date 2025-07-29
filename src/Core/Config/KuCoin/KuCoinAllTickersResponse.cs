using System.Text.Json.Serialization;

namespace Core.Config.KuCoin;

public class KuCoinAllTickersResponse
{
    [JsonPropertyName("time")]
    public long Time { get; set; }

    [JsonPropertyName("ticker")]
    public List<KuCoinTicker>? Ticker { get; set; }
}