using System.Text.Json.Serialization;

namespace Core.Config.Bybit._24hStat;

public class BybitDayStatTicker
{

    [JsonPropertyName("lastPrice")]
    public string LastPrice { get; set; } = null!;

    [JsonPropertyName("prevPrice24h")]
    public string PrevPrice24h { get; set; } = null!;

    [JsonPropertyName("price24hPcnt")]
    public string Price24hPcnt { get; set; } = null!;

    [JsonPropertyName("highPrice24h")]
    public string HighPrice24h { get; set; } = null!;

    [JsonPropertyName("lowPrice24h")]
    public string LowPrice24h { get; set; } = null!;

    [JsonPropertyName("volume24h")]
    public string Volume24h { get; set; } = null!;

}