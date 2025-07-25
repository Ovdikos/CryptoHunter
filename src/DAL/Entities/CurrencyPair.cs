namespace DAL.Entities;

public partial class CurrencyPair
{
    public int PairId { get; set; }

    public int FromCurrency { get; set; }

    public int ToCurrency { get; set; }

    public virtual ICollection<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();

    public virtual Currency FromCurrencyNavigation { get; set; } = null!;

    public virtual Currency ToCurrencyNavigation { get; set; } = null!;
}
