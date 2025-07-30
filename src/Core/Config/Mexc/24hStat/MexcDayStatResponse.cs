using System.Text.Json.Serialization;

namespace Core.Config.Mexc._24hStat;

public class MexcDayStatResponse
{
    [JsonPropertyName("priceChangePercent")]
    public string PriceChangePercent { get; set; } = null!;

    [JsonPropertyName("lastPrice")]
    public string LastPrice { get; set; } = null!;

    [JsonPropertyName("openPrice")]
    public string OpenPrice { get; set; } = null!;

    [JsonPropertyName("highPrice")]
    public string HighPrice { get; set; } = null!;

    [JsonPropertyName("lowPrice")]
    public string LowPrice { get; set; } = null!;

    [JsonPropertyName("volume")]
    public string Volume { get; set; } = null!;

    [JsonPropertyName("quoteVolume")]
    public string QuoteVolume { get; set; } = null!;
}