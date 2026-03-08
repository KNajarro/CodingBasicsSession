using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
using CodingBasics.Domain.AdventureWorks.Entities;
using CodingBasics.Infrastructure.Persistence;

namespace CodingBasics.Infrastructure.Repositories;

/// <summary>
/// Person repository implementation using EF Core.
/// </summary>
public sealed class PersonRepository : IPersonRepository
{
  private readonly AdventureWorksDbContext _context;

  public PersonRepository(AdventureWorksDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Person>> GetAllAsync(CancellationToken ct = default)
  {
    return await _context.People.ToListAsync(ct);
  }

  public async Task<IEnumerable<Person>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
  {
    var query = _context.People.AsQueryable();

    if (!string.IsNullOrWhiteSpace(name))
      query = query.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));

    if (!string.IsNullOrWhiteSpace(personType))
      query = query.Where(p => p.PersonType == personType);

    return await query.ToListAsync(ct);
  }

  public async Task<Person> CreateAsync(Person entity, CancellationToken ct = default)
  {
    // Get next BusinessEntityID (AdventureWorks doesn't auto-generate this)
    var maxId = await _context.People
        .MaxAsync(p => (int?)p.BusinessEntityID, ct) ?? 0;
    entity.BusinessEntityID = maxId + 1;

    // Use transaction to ensure consistency
    await using var transaction = await _context.Database.BeginTransactionAsync(ct);
    try
    {
      // BusinessEntityID is not IDENTITY, so we can insert directly
      _context.People.Add(entity);
      await _context.SaveChangesAsync(ct);

      await transaction.CommitAsync(ct);
      return entity;
    }
    catch
    {
      await transaction.RollbackAsync(ct);
      throw;
    }
  }

  public async Task<Person> UpdateAsync(int id, Person entity, CancellationToken ct = default)
  {
    var existing = await _context.People.FindAsync(new object[] { id }, ct);
    if (existing == null)
      throw new KeyNotFoundException($"Person with ID {id} not found");

    existing.PersonType = entity.PersonType;
    existing.Title = entity.Title;
    existing.FirstName = entity.FirstName;
    existing.MiddleName = entity.MiddleName;
    existing.LastName = entity.LastName;
    existing.Suffix = entity.Suffix;
    existing.EmailPromotion = entity.EmailPromotion;
    existing.ModifiedDate = DateTime.UtcNow;

    await _context.SaveChangesAsync(ct);
    return existing;
  }

  public async Task DeleteAsync(int id, CancellationToken ct = default)
  {
    var entity = await _context.People.FindAsync(new object[] { id }, ct);
    if (entity == null)
      throw new KeyNotFoundException($"Person with ID {id} not found");

    _context.People.Remove(entity);
    await _context.SaveChangesAsync(ct);
  }
}

/// <summary>
/// Product repository implementation using EF Core.
/// </summary>
public sealed class ProductRepository : IProductRepository
{
  private readonly AdventureWorksDbContext _context;

  public ProductRepository(AdventureWorksDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
  {
    return await _context.Products.ToListAsync(ct);
  }

  public async Task<IEnumerable<Product>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
  {
    var query = _context.Products.AsQueryable();

    if (!string.IsNullOrWhiteSpace(name))
      query = query.Where(p => p.Name.Contains(name));

    if (!string.IsNullOrWhiteSpace(categoryName))
    {
      // Join with ProductSubcategory and ProductCategory to filter by category name
      query = from p in query
              join sc in _context.ProductSubcategories on p.ProductSubcategoryID equals sc.ProductSubcategoryID
              join c in _context.ProductCategories on sc.ProductCategoryID equals c.ProductCategoryID
              where c.Name.Contains(categoryName)
              select p;
    }

    return await query.ToListAsync(ct);
  }

  public async Task<Product> CreateAsync(Product entity, CancellationToken ct = default)
  {
    // Get next ProductID
    var maxId = await _context.Products.MaxAsync(p => (int?)p.ProductID, ct) ?? 0;
    entity.ProductID = maxId + 1;

    // Use IDENTITY_INSERT to insert explicit ID value (required due to triggers preventing OUTPUT clause)
    await using var transaction = await _context.Database.BeginTransactionAsync(ct);
    try
    {
      await _context.Database.ExecuteSqlRawAsync(
        "SET IDENTITY_INSERT [Production].[Product] ON", ct);

      _context.Products.Add(entity);
      await _context.SaveChangesAsync(ct);

      await _context.Database.ExecuteSqlRawAsync(
        "SET IDENTITY_INSERT [Production].[Product] OFF", ct);

      await transaction.CommitAsync(ct);
      return entity;
    }
    catch
    {
      await transaction.RollbackAsync(ct);
      throw;
    }
  }

  public async Task<Product> UpdateAsync(int id, Product entity, CancellationToken ct = default)
  {
    var existing = await _context.Products.FindAsync(new object[] { id }, ct);
    if (existing == null)
      throw new KeyNotFoundException($"Product with ID {id} not found");

    existing.Name = entity.Name;
    existing.ProductNumber = entity.ProductNumber;
    existing.Color = entity.Color;
    existing.ListPrice = entity.ListPrice;
    existing.StandardCost = entity.StandardCost;
    existing.SafetyStockLevel = entity.SafetyStockLevel;
    existing.ReorderPoint = entity.ReorderPoint;
    existing.ModifiedDate = DateTime.UtcNow;

    await _context.SaveChangesAsync(ct);
    return existing;
  }

  public async Task DeleteAsync(int id, CancellationToken ct = default)
  {
    var entity = await _context.Products.FindAsync(new object[] { id }, ct);
    if (entity == null)
      throw new KeyNotFoundException($"Product with ID {id} not found");

    _context.Products.Remove(entity);
    await _context.SaveChangesAsync(ct);
  }
}