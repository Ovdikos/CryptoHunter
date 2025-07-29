using System.Text.Json.Serialization;

namespace Core.Config.KuCoin;

public class KuCoinResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;    

    [JsonPropertyName("data")]
    public KuCoinAllTickersResponse? Data { get; set; }
}