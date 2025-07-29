using System.Text.Json.Serialization;

namespace Core.Config.KuCoin;

public class KuCoinBookResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("data")]
    public KuCoinBookTicker Data { get; set; } = null!;
}