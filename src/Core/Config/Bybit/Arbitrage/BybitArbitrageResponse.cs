using System.Text.Json.Serialization;

namespace Core.Config.Bybit.Arbitrage;

public class BybitArbitrageResponse<T>
{

    [JsonPropertyName("result")]
    public T Result { get; set; } = default!;
}