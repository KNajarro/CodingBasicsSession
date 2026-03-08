using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
using CodingBasics.Infrastructure.Persistence;

namespace CodingBasics.Infrastructure.Repositories;

/// <summary>
/// Person repository implementation using EF Core.
/// TODO (Workshop): Implement full CRUD methods using AdventureWorksDbContext.
/// </summary>
public sealed class PersonRepository : IPersonRepository
{
    private readonly AdventureWorksDbContext _context;

    public PersonRepository(AdventureWorksDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default)
    {
        // TODO (Workshop): Query all people and map to PersonDto
        // Hint: Use _context.People.Select(...).ToListAsync(ct)

        return await _context.People
            .Select(p => new PersonDto
            {
                BusinessEntityID = p.BusinessEntityID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PersonType = p.PersonType
            })
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        var query = _context.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p =>
                p.FirstName.Contains(name) ||
                p.LastName.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(personType))
        {
            query = query.Where(p => p.PersonType == personType);
        }

        return await query
            .Select(p => new PersonDto
            {
                BusinessEntityID = p.BusinessEntityID,
                PersonType = p.PersonType,
                Title = p.Title,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Suffix = p.Suffix,
                EmailPromotion = p.EmailPromotion
            })
            .ToListAsync(ct);
    }

    // TODO (Workshop): Implement CreateAsync, UpdateAsync, DeleteAsync for Person
}

/// <summary>
/// Product repository implementation using EF Core.
/// TODO (Workshop): Implement query methods with category joins.
/// </summary>
public sealed class ProductRepository : IProductRepository
{
    private readonly AdventureWorksDbContext _context;

    public ProductRepository(AdventureWorksDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await (
            from p in _context.Products
            join sc in _context.ProductSubcategories
                on p.ProductSubcategoryID equals sc.ProductSubcategoryID into scGroup
            from sc in scGroup.DefaultIfEmpty()
            join c in _context.ProductCategories
                on sc.ProductCategoryID equals c.ProductCategoryID into cGroup
            from c in cGroup.DefaultIfEmpty()
            select new ProductDto
            {
                ProductID = p.ProductID,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                Color = p.Color,
                ListPrice = p.ListPrice,
                SafetyStockLevel = p.SafetyStockLevel,
                CategoryName = c != null ? c.Name : null,
                SubcategoryName = sc != null ? sc.Name : null
            }
        ).ToListAsync(ct);
    }
    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        var query =
            from p in _context.Products
            join sc in _context.ProductSubcategories
                on p.ProductSubcategoryID equals sc.ProductSubcategoryID into scGroup
            from sc in scGroup.DefaultIfEmpty()
            join c in _context.ProductCategories
                on sc.ProductCategoryID equals c.ProductCategoryID into cGroup
            from c in cGroup.DefaultIfEmpty()
            select new ProductDto
            {
                ProductID = p.ProductID,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                Color = p.Color,
                ListPrice = p.ListPrice,
                SafetyStockLevel = p.SafetyStockLevel,
                CategoryName = c != null ? c.Name : null,
                SubcategoryName = sc != null ? sc.Name : null
            };

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            query = query.Where(p => p.CategoryName != null && p.CategoryName.Contains(categoryName));
        }

        return await query.ToListAsync(ct);
    }
}