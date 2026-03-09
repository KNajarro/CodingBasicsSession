using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
using CodingBasics.Domain.AdventureWorks.Entities;
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
            query = query.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));

        if (!string.IsNullOrWhiteSpace(personType))
            query = query.Where(p => p.PersonType == personType);

        return await query.Select(p => new PersonDto
        {
            BusinessEntityID = p.BusinessEntityID,
            FirstName = p.FirstName,
            LastName = p.LastName,
            PersonType = p.PersonType,
            Title = p.Title,
            MiddleName = p.MiddleName,
            Suffix = p.Suffix,
            EmailPromotion = p.EmailPromotion
        }).ToListAsync(ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        var person = new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PersonType = dto.PersonType,
            Title = dto.Title,
            MiddleName = dto.MiddleName,
            Suffix = dto.Suffix,
            EmailPromotion = dto.EmailPromotion,
            NameStyle = false,
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.UtcNow
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync(ct);

        dto.BusinessEntityID = person.BusinessEntityID;
        return dto;
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        var person = await _context.People.FindAsync(new object[] { id }, cancellationToken: ct);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {id} not found");

        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;
        person.PersonType = dto.PersonType;
        person.Title = dto.Title;
        person.MiddleName = dto.MiddleName;
        person.Suffix = dto.Suffix;
        person.EmailPromotion = dto.EmailPromotion;
        person.ModifiedDate = DateTime.UtcNow;

        _context.People.Update(person);
        await _context.SaveChangesAsync(ct);

        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var person = await _context.People.FindAsync(new object[] { id }, cancellationToken: ct);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {id} not found");

        _context.People.Remove(person);
        await _context.SaveChangesAsync(ct);
    }
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
        return await (from p in _context.Products
                      join sc in _context.ProductSubcategories
                          on p.ProductSubcategoryID equals sc.ProductSubcategoryID into scGroup
                      from sc in scGroup.DefaultIfEmpty()
                      join c in _context.ProductCategories
                          on sc.ProductCategoryID equals c.ProductCategoryID into cGroup
                      from c in cGroup.DefaultIfEmpty()
                      select new ProductDto
                      {
                          ProductID        = p.ProductID,
                          Name             = p.Name,
                          ProductNumber    = p.ProductNumber,
                          Color            = p.Color,
                          ListPrice        = p.ListPrice,
                          SafetyStockLevel = p.SafetyStockLevel,
                          ReorderPoint     = p.ReorderPoint,
                          CategoryName     = c != null ? c.Name : null,
                          SubcategoryName  = sc != null ? sc.Name : null
                      }).ToListAsync(ct);
    }

     public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
     {
         var query = (from p in _context.Products
                      join sc in _context.ProductSubcategories
                          on p.ProductSubcategoryID equals sc.ProductSubcategoryID into scGroup
                      from sc in scGroup.DefaultIfEmpty()
                      join c in _context.ProductCategories
                          on sc.ProductCategoryID equals c.ProductCategoryID into cGroup
                      from c in cGroup.DefaultIfEmpty()
                      select new ProductDto
                      {
                          ProductID        = p.ProductID,
                          Name             = p.Name,
                          ProductNumber    = p.ProductNumber,
                          Color            = p.Color,
                          ListPrice        = p.ListPrice,
                          SafetyStockLevel = p.SafetyStockLevel,
                          ReorderPoint     = p.ReorderPoint,
                          CategoryName     = c != null ? c.Name : null,
                          SubcategoryName  = sc != null ? sc.Name : null
                      }).AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(p => p.CategoryName == categoryName);

        return await query.ToListAsync(ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var product = new Product
        {
            Name = dto.Name,
            ProductNumber = dto.ProductNumber,
            Color = dto.Color,
            ListPrice = dto.ListPrice,
            SafetyStockLevel = 10,  // Default safety stock level
            ReorderPoint = 10,      // Default reorder point
            StandardCost = dto.ListPrice * 0.5m,  // Default cost as 50% of list price
            MakeFlag = false,
            FinishedGoodsFlag = true,
            DaysToManufacture = 0,
            SellStartDate = DateTime.UtcNow,
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);

        dto.ProductID = product.ProductID;
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken: ct);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        product.Name = dto.Name;
        product.ProductNumber = dto.ProductNumber;
        product.Color = dto.Color;
        product.ListPrice = dto.ListPrice;
        product.ModifiedDate = DateTime.UtcNow;

        _context.Products.Update(product);
        await _context.SaveChangesAsync(ct);

        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken: ct);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }
}