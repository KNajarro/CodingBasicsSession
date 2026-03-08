using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.AdventureWorks.Entities;

namespace CodingBasics.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for AdventureWorks database.
/// Configured with proper table/schema mappings, keys, and relationships for full CRUD support.
/// </summary>
public sealed class AdventureWorksDbContext : DbContext
{
    public AdventureWorksDbContext(DbContextOptions<AdventureWorksDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductSubcategory> ProductSubcategories => Set<ProductSubcategory>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity mappings to AdventureWorks tables with correct schemas, keys, and relationships.
        //
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person", "Person");
            entity.HasKey(e => e.BusinessEntityID);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "Production");
            entity.HasKey(e => e.ProductID);
            entity.HasOne<ProductSubcategory>()
                .WithMany()
                .HasForeignKey(e => e.ProductSubcategoryID)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProductSubcategory>(entity =>
        {
            entity.ToTable("ProductSubcategory", "Production");
            entity.HasKey(e => e.ProductSubcategoryID);
            entity.HasOne<ProductCategory>()
                .WithMany()
                .HasForeignKey(e => e.ProductCategoryID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategory", "Production");
            entity.HasKey(e => e.ProductCategoryID);
        });
    }
}