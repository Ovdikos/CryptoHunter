using System.Text.Json.Serialization;

namespace Core.Config.BingX.Arbitrage;

public class BingxArbitrageResponse
{
    [JsonPropertyName("data")]
    public List<BingxArbitrageTicker>? Data { get; set; }
}