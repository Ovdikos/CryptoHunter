namespace BLL.DTOs;

public class CurrencyPairRateDto
{
    public string PairSymbol   { get; set; }  
    public decimal Rate        { get; set; }  
    public string ExchangeName { get; set; }  
}