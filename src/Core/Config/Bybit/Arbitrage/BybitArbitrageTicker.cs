using System.Text.Json.Serialization;

namespace Core.Config.Bybit.Arbitrage;

public class BybitArbitrageTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("bid1Price")]
    public string Bid1Price { get; set; } = null!;

    [JsonPropertyName("ask1Price")]
    public string Ask1Price { get; set; } = null!;
}