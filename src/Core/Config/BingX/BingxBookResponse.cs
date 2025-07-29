using System.Text.Json.Serialization;

namespace Core.Config.BingX;

public class BingxBookResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("data")]
    public List<BingxBookTicker>? Data { get; set; }
}