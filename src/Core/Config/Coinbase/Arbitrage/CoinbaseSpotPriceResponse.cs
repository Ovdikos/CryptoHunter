using System.Text.Json.Serialization;

namespace Core.Config.Coinbase.Arbitrage;

public class CoinbaseSpotPriceResponse
{
    [JsonPropertyName("data")]
    public CoinbaseSpotPrice Data { get; set; } = null!;
}