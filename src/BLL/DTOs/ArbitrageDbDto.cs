namespace BLL.DTOs;

public class ArbitrageDbDto
{
    public string Exchange { get; set; } = null!;
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
}