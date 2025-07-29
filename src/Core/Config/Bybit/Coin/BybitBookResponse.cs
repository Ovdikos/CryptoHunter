using System.Text.Json.Serialization;

namespace Core.Config.Bybit.Coin;

public class BybitBookResponse<T>
{
    [JsonPropertyName("result")]
    public T result { get; set; } = default!;
}