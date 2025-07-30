namespace BLL.DTOs._24hStat;

public class MarketSummaryDto
{
    public string Pair                          { get; set; } = null!;
    public List<Exchange24hDto> PerExchange     { get; set; } = new();
    public Exchange24hDto Aggregated            { get; set; } = null!;
}