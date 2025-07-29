namespace Core.Config.Mexc;

public class MexcBookResponse
{
    public int code { get; set; }
    public string message { get; set; } = null!;
    public List<MexcBookTicker> Data { get; set; } = new();
}