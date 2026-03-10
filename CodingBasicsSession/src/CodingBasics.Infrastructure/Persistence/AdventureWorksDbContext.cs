using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.AdventureWorks.Entities;

namespace CodingBasics.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for AdventureWorks database.
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

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person", "Person");
            entity.HasKey(e => e.BusinessEntityID);
        });
        //
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "Production");
            entity.HasKey(e => e.ProductID);
        });
        //
        modelBuilder.Entity<ProductSubcategory>(entity =>
        {
            entity.ToTable("ProductSubcategory", "Production");
            entity.HasKey(e => e.ProductSubcategoryID);
        });
        //
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategory", "Production");
            entity.HasKey(e => e.ProductCategoryID);
        });
    }
}