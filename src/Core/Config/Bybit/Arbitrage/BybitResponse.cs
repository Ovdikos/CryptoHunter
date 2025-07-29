using System.Text.Json.Serialization;

namespace Core.Config.Bybit.Arbitrage;

public class BybitResponse<T>
{
    [JsonPropertyName("retCode")]
    public int RetCode { get; set; }

    [JsonPropertyName("retMsg")]
    public string RetMsg { get; set; } = null!;

    [JsonPropertyName("result")]
    public T Result { get; set; } = default!;
}