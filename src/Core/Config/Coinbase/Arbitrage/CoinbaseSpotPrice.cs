using System.Text.Json.Serialization;

namespace Core.Config.Coinbase.Arbitrage;

public class CoinbaseSpotPrice
{
    [JsonPropertyName("base")]
    public string BaseCurrency { get; set; } = null!;

    [JsonPropertyName("currency")]
    public string QuoteCurrency { get; set; } = null!;

    [JsonPropertyName("amount")]
    public string Amount { get; set; } = null!;
}