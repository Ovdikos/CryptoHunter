namespace BLL.DTOs._24hStat;

public class Exchange24hDto
{
    public string Exchange           { get; set; } = null!;
    public string Pair               { get; set; } = null!;
    public decimal Open              { get; set; }
    public decimal High              { get; set; }
    public decimal Low               { get; set; }
    public decimal Close             { get; set; }
    public decimal Volume            { get; set; }
    public decimal PriceChangePct    { get; set; }
    public decimal WeightedAvgPrice  { get; set; }
}