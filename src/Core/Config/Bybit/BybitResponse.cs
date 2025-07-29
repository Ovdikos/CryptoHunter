using System.Text.Json.Serialization;

namespace Core.Config.Bybit;

public class BybitResponse
{
    [JsonPropertyName("result")]
    public ResultContainer? Result { get; set; }

    public class ResultContainer
    {
        [JsonPropertyName("list")]
        public List<BybitTicker>? List { get; set; }
    }
}