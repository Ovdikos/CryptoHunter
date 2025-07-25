using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class CryptoContext : DbContext
{
    public CryptoContext()
    {
    }

    public CryptoContext(DbContextOptions<CryptoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyPair> CurrencyPairs { get; set; }

    public virtual DbSet<Exchange> Exchanges { get; set; }

    public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }

    public virtual DbSet<FetchSchedule> FetchSchedules { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        // => optionsBuilder.UseSqlServer("Server=localhost,1436;User Id=sa;Password=2f$A_Xs9;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currenci__14470AF0FF5FA539");

            entity.HasIndex(e => e.Symbol, "UQ__Currenci__B7CC3F0164EA97D3").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(10);
        });

        modelBuilder.Entity<CurrencyPair>(entity =>
        {
            entity.HasKey(e => e.PairId).HasName("PK__Currency__B543F7CCEDA10196");

            entity.HasIndex(e => new { e.FromCurrency, e.ToCurrency }, "UQ__Currency__879912EE669D36B8").IsUnique();

            entity.HasOne(d => d.FromCurrencyNavigation).WithMany(p => p.CurrencyPairFromCurrencyNavigations)
                .HasForeignKey(d => d.FromCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyP__FromC__278EDA44");

            entity.HasOne(d => d.ToCurrencyNavigation).WithMany(p => p.CurrencyPairToCurrencyNavigations)
                .HasForeignKey(d => d.ToCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyP__ToCur__2882FE7D");
        });

        modelBuilder.Entity<Exchange>(entity =>
        {
            entity.HasKey(e => e.ExchangeId).HasName("PK__Exchange__72E6008B4AFE8422");

            entity.Property(e => e.ApiBaseUrl).HasMaxLength(200);
            entity.Property(e => e.ApiKey).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ExchangeRate>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Exchange__58A7CF5CB4381646");

            entity.Property(e => e.Rate).HasColumnType("decimal(28, 8)");
            entity.Property(e => e.RetrievedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Exchange).WithMany(p => p.ExchangeRates)
                .HasForeignKey(d => d.ExchangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExchangeR__Excha__2B5F6B28");

            entity.HasOne(d => d.Pair).WithMany(p => p.ExchangeRates)
                .HasForeignKey(d => d.PairId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExchangeR__PairI__2C538F61");
        });

        modelBuilder.Entity<FetchSchedule>(entity =>
        {
            entity.HasKey(e => e.ExchangeId).HasName("PK__FetchSch__72E6008B38351E37");

            entity.Property(e => e.ExchangeId).ValueGeneratedNever();
            entity.Property(e => e.IntervalSec).HasDefaultValue(60);

            entity.HasOne(d => d.Exchange).WithOne(p => p.FetchSchedule)
                .HasForeignKey<FetchSchedule>(d => d.ExchangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FetchSche__Excha__30242045");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
