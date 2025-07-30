namespace BLL.DTOs.PriceChangeIn24h;

public class TopPairDto
{
    public string Pair { get; set; } = null!;
    public decimal PriceChangePct { get; set; }
    public decimal Volume { get; set; }
}