using System.Text.Json.Serialization;

namespace Core.Config.BingX;

public class BingxResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public BingxTicker? Data { get; set; }
}