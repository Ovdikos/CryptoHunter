using System.Text.Json.Serialization;

namespace Core.Config.Coinbase.Arbitrage;

public class CoinbaseProductTicker
{
    [JsonPropertyName("ask")]
    public string Ask { get; set; } = null!;

    [JsonPropertyName("bid")]
    public string Bid { get; set; } = null!;

    [JsonPropertyName("price")]
    public string Price { get; set; } = null!;

    [JsonPropertyName("volume")]
    public string Volume { get; set; } = null!;

    [JsonPropertyName("trade_id")]
    public long TradeId { get; set; }

    [JsonPropertyName("size")]
    public string Size { get; set; } = null!;

    [JsonPropertyName("time")]
    public string Time { get; set; } = null!;

    [JsonPropertyName("rfq_volume")]
    public string? RfqVolume { get; set; }
}