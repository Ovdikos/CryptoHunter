using System.Text.Json.Serialization;

namespace Core.Config.KuCoin._24hStat;

public class KuCoinDayStatResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("data")]
    public KuCoinDayStatData Data { get; set; } = null!;
}