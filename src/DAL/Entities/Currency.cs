namespace DAL.Entities;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string Symbol { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<CurrencyPair> CurrencyPairFromCurrencyNavigations { get; set; } = new List<CurrencyPair>();

    public virtual ICollection<CurrencyPair> CurrencyPairToCurrencyNavigations { get; set; } = new List<CurrencyPair>();
}
