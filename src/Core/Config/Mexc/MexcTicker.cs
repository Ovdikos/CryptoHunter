using System.Text.Json.Serialization;

namespace Core.Config.Mexc;

public class MexcTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;   

    [JsonPropertyName("price")]
    public string Price  { get; set; } = null!;   
}