namespace DAL.Entities;

public partial class FetchSchedule
{
    public int ExchangeId { get; set; }

    public int IntervalSec { get; set; }

    public virtual Exchange Exchange { get; set; } = null!;
}
