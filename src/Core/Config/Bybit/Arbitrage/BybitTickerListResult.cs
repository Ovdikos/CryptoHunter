using System.Text.Json.Serialization;

namespace Core.Config.Bybit.Arbitrage;

public class BybitTickerListResult
{
    [JsonPropertyName("list")]
    public List<BybitTicker> List { get; set; } = new();
}