using System.Text.Json.Serialization;

namespace Core.Config.BingX;

public class BingxSwapPriceResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public BingxSwapPriceData? Data { get; set; }
}