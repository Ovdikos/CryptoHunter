using System.Text.Json.Serialization;

namespace Core.Config.Bybit._24hStat;

public class BybitDayStatResult
{
    [JsonPropertyName("list")]
    public List<BybitDayStatTicker> List { get; set; } = new();
}