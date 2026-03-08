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

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure Person entity
    // Disable OUTPUT clause due to database triggers on Person.Person table
    // BusinessEntityID is not auto-generated, must be set manually
    modelBuilder.Entity<Person>(entity =>
    {
      entity.ToTable("Person", "Person", tb => tb.UseSqlOutputClause(false));
      entity.HasKey(e => e.BusinessEntityID);
      entity.Property(e => e.BusinessEntityID).ValueGeneratedNever();
    });

    // Configure Product entity
    // Disable OUTPUT clause due to database triggers on Production.Product table
    // ProductID must be set manually due to OUTPUT clause limitation
    modelBuilder.Entity<Product>(entity =>
    {
      entity.ToTable("Product", "Production", tb => tb.UseSqlOutputClause(false));
      entity.HasKey(e => e.ProductID);
      entity.Property(e => e.ProductID).ValueGeneratedNever();
      entity.Property(e => e.ListPrice).HasPrecision(19, 4);
      entity.Property(e => e.StandardCost).HasPrecision(19, 4);
      entity.Property(e => e.Weight).HasPrecision(8, 2);
    });

    modelBuilder.Entity<ProductSubcategory>(entity =>
    {
      entity.ToTable("ProductSubcategory", "Production");
      entity.HasKey(e => e.ProductSubcategoryID);
    });

    modelBuilder.Entity<ProductCategory>(entity =>
    {
      entity.ToTable("ProductCategory", "Production");
      entity.HasKey(e => e.ProductCategoryID);
    });
  }
}