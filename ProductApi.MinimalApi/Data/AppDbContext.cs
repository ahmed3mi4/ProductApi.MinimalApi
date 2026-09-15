using Microsoft.EntityFrameworkCore;
using ProductApi.MinimalApi.Models;

namespace ProductApi.MinimalApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

            entity.Property(e => e.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();  

            entity.Property(e => e.CreatedAtUtc)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            )
            .IsRequired();

            entity.ToTable(table => {
                table.HasCheckConstraint(
           "CK_Products_Price_Positive",
           "[Price] > 0");
                table.HasCheckConstraint(
          "CK_Products_Name_NotBlank",
           "LEN(LTRIM(RTRIM([Name]))) > 0");   
            });


        });
    }
}
