namespace DAL.Entities;

public partial class CurrencyPair
{
    public int PairId { get; set; }

    public int FromCurrency { get; set; }

    public int ToCurrency { get; set; }

    public virtual Currency FromCurrencyNavigation { get; set; } = null!;

    public virtual Currency ToCurrencyNavigation { get; set; } = null!;
}
