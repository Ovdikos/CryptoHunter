using System.Text.Json.Serialization;

namespace Core.Config.Bybit.Arbitrage;

public class BybitArbitrageResult
{
    [JsonPropertyName("list")]
    public List<BybitArbitrageTicker> List { get; set; } = new();
}