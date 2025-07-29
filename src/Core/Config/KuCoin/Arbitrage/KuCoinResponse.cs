using System.Text.Json.Serialization;

namespace Core.Config.KuCoin.Arbitrage;

public class KuCoinResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("data")]
    public KuCoinAllTickersResponse Data { get; set; } = null!;
}