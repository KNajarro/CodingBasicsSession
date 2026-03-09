using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.AdventureWorks.Entities;

namespace CodingBasics.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for AdventureWorks database.
/// TODO (Workshop): Complete OnModelCreating with proper table/schema mappings and ensure full CRUD support for entities.
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
    public DbSet<BusinessEntity> BusinessEntities => Set<BusinessEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TODO (Workshop): Map entities to AdventureWorks tables with correct schemas and keys.
        //
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

        modelBuilder.Entity<BusinessEntity>(entity =>
        {
            entity.ToTable("BusinessEntity", "Person");
            entity.HasKey(e => e.BusinessEntityID);
            entity.Property(e => e.BusinessEntityID).ValueGeneratedOnAdd();
        });
    }
}