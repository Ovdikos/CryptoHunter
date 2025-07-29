using System.Text.Json.Serialization;

namespace Core.Config.Coinbase;

public class CoinbaseTicker
{
    [JsonPropertyName("base")]     public string Base     { get; set; } = null!;
    [JsonPropertyName("currency")] public string Currency { get; set; } = null!;
    [JsonPropertyName("amount")]   public string Amount   { get; set; } = null!;
}