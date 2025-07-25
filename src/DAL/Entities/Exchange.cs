namespace DAL.Entities;

public partial class Exchange
{
    public int ExchangeId { get; set; }

    public string Name { get; set; } = null!;

    public string ApiBaseUrl { get; set; } = null!;

    public string? ApiKey { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();

    public virtual FetchSchedule? FetchSchedule { get; set; }
}
