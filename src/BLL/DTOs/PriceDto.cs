namespace BLL.DTOs;

public class PriceDto
{
    public string Pair { get; set; } = null!;
    
    public string Exchange { get; set; } = null!;

    public decimal Price { get; set; }
}