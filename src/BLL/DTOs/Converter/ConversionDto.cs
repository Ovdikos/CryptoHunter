namespace BLL.DTOs.Converter;

public class ConversionDto
{
    public string FromPair      { get; set; } = null!; 
    public string ToPair        { get; set; } = null!;
    public decimal FromAmount   { get; set; }
    public decimal UsdtAmount   { get; set; }
    public decimal ToAmount     { get; set; }
    public decimal BestBid      { get; set; }
    public string BidExchange   { get; set; } = null!;
    public decimal BestAsk      { get; set; }
    public string AskExchange   { get; set; } = null!;
}