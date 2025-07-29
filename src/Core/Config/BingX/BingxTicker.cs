using System.Text.Json.Serialization;

namespace Core.Config.BingX;

public class BingxTicker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;   

    [JsonPropertyName("price")]
    public string Price  { get; set; } = null!; 
}