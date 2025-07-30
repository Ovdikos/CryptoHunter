using System.Text.Json.Serialization;

namespace Core.Config.BingX._24hStat;

public class BingxDayStat
{
    [JsonPropertyName("data")]
    public List<List<decimal>> Data { get; set; } = new();
}