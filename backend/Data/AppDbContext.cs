using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<LocationDetail> LocationDetails => Set<LocationDetail>();
    public DbSet<PurchaseBill> PurchaseBills => Set<PurchaseBill>();
    public DbSet<PurchaseBillItem> PurchaseBillItems => Set<PurchaseBillItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationDetail>(e =>
        {
            e.ToTable("Location_Details");
            e.Property(x => x.Location_Code).HasMaxLength(50);
            e.Property(x => x.Location_Name).HasMaxLength(200);
            e.HasIndex(x => x.Location_Code).IsUnique();
        });

        modelBuilder.Entity<PurchaseBill>(e =>
        {
            e.ToTable("Purchase_Bill");
            e.Property(x => x.Created_By).HasMaxLength(200);
            e.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.Purchase_Bill_Id);
        });

        modelBuilder.Entity<PurchaseBillItem>(e =>
        {
            e.ToTable("Purchase_Bill_Item");
            e.Property(x => x.Item).HasMaxLength(100);
            e.Property(x => x.Batch).HasMaxLength(200);
        });

        foreach (var p in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal)))
        {
            p.SetPrecision(18);
            p.SetScale(2);
        }
    }
}
