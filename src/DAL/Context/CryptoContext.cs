using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currenci__14470AF0181FE40A");

            entity.HasIndex(e => e.Symbol, "UQ__Currenci__B7CC3F01F22CC4FD").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(10);
        });

        modelBuilder.Entity<CurrencyPair>(entity =>
        {
            entity.HasKey(e => e.PairId).HasName("PK__Currency__B543F7CC58C827FB");

            entity.HasIndex(e => new { e.FromCurrency, e.ToCurrency }, "UQ__Currency__879912EEBA3BF471").IsUnique();

            entity.HasOne(d => d.FromCurrencyNavigation).WithMany(p => p.CurrencyPairFromCurrencyNavigations)
                .HasForeignKey(d => d.FromCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyP__FromC__3C89F72A");

            entity.HasOne(d => d.ToCurrencyNavigation).WithMany(p => p.CurrencyPairToCurrencyNavigations)
                .HasForeignKey(d => d.ToCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyP__ToCur__3D7E1B63");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
