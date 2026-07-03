using Microsoft.EntityFrameworkCore;
using DepoYonetimi.API.Entities;

namespace DepoYonetimi.API.Data;

public class DepoDbContext : DbContext
{
    public DepoDbContext(DbContextOptions<DepoDbContext> options) : base(options)
    {
    }

    public DbSet<Urun> Urunler { get; set; } = null!;
    public DbSet<Depo> Depolar { get; set; } = null!;
    public DbSet<Stok> Stoklar { get; set; } = null!;
    public DbSet<DepoHareket> DepoHareketleri { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Urun configuration
        modelBuilder.Entity<Urun>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UrunKodu).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UrunAdi).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CompanyId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Kategori).HasMaxLength(100);
            entity.Property(e => e.Birim).HasMaxLength(20);
            entity.Property(e => e.BirimFiyat).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Barkod).HasMaxLength(100);

            entity.HasIndex(e => new { e.CompanyId, e.UrunKodu }).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Depo configuration
        modelBuilder.Entity<Depo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DepoKodu).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DepoAdi).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CompanyId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Konum).HasMaxLength(200);
            entity.Property(e => e.Bolge).HasMaxLength(50);
            entity.Property(e => e.Raf).HasMaxLength(50);

            entity.HasIndex(e => new { e.CompanyId, e.DepoKodu }).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Stok configuration
        modelBuilder.Entity<Stok>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyId).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.Urun)
                .WithMany(u => u.Stoklar)
                .HasForeignKey(e => e.UrunId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Depo)
                .WithMany(d => d.Stoklar)
                .HasForeignKey(e => e.DepoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.CompanyId, e.UrunId, e.DepoId }).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // DepoHareket configuration
        modelBuilder.Entity<DepoHareket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.HareketTipi).IsRequired().HasMaxLength(20);
            entity.Property(e => e.IslemYapan).HasMaxLength(100);
            entity.Property(e => e.ReferansNo).HasMaxLength(50);

            entity.HasOne(e => e.Urun)
                .WithMany(u => u.Hareketler)
                .HasForeignKey(e => e.UrunId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Depo)
                .WithMany(d => d.Hareketler)
                .HasForeignKey(e => e.DepoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.CompanyId);
            entity.HasIndex(e => e.HareketTarihi);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}
