namespace DAL.Entities;

public partial class ExchangeRate
{
    public long RateId { get; set; }

    public int ExchangeId { get; set; }

    public int PairId { get; set; }

    public decimal Rate { get; set; }

    public DateTime RetrievedAt { get; set; }

    public virtual Exchange Exchange { get; set; } = null!;

    public virtual CurrencyPair Pair { get; set; } = null!;
}
