using System.Text.Json.Serialization;

namespace Core.Config.KuCoin.Arbitrage;

public class KuCoinArbitrageTicker
{
    [JsonPropertyName("bestBid")]
    public string BestBid { get; set; } = null!;

    [JsonPropertyName("bestAsk")]
    public string BestAsk { get; set; } = null!;
}