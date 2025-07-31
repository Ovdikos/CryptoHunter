using System.Text.Json.Serialization;

namespace Core.Config.KuCoin.Arbitrage;

public class KuCoinArbitrageResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("data")]
    public KuCoinArbitrageTicker Data { get; set; } = null!;
}