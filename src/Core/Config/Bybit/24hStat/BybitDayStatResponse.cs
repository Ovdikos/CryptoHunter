using System.Text.Json.Serialization;

namespace Core.Config.Bybit._24hStat;

public class BybitDayStatResponse
{
    [JsonPropertyName("result")]
    public BybitDayStatResult Result { get; set; } = null!;
}