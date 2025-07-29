using System.Text.Json.Serialization;

namespace Core.Config.Coinbase;

public class CoinbaseResponse
{
    [JsonPropertyName("data")]
    public CoinbaseTicker? Data { get; set; }
}