using System.Text.Json.Serialization;

namespace Core.Config.KuCoin._24hStat;

public class KuCoinDayStatData
{
    [JsonPropertyName("changeRate")]
    public string ChangeRate { get; set; } = null!;

    [JsonPropertyName("changePrice")]
    public string ChangePrice { get; set; } = null!;

    [JsonPropertyName("high")]
    public string High { get; set; } = null!;

    [JsonPropertyName("low")]
    public string Low { get; set; } = null!;

    [JsonPropertyName("vol")]
    public string Volume { get; set; } = null!;

    [JsonPropertyName("last")]
    public string Last { get; set; } = null!;

    [JsonPropertyName("averagePrice")]
    public string AveragePrice { get; set; } = null!;
}