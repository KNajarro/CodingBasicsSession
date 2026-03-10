using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
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

    public async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.People
            .Select(p => new PersonDto
            {
                BusinessEntityID = p.BusinessEntityID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                Title = p.Title,
                Suffix = p.Suffix,
                PersonType = p.PersonType,
                EmailPromotion = p.EmailPromotion
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
            MiddleName = p.MiddleName,
            Title = p.Title,
            Suffix = p.Suffix,
            PersonType = p.PersonType,
            EmailPromotion = p.EmailPromotion
        }).ToListAsync(ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        var person = new Domain.AdventureWorks.Entities.Person
        {
            PersonType = dto.PersonType,
            NameStyle = false,
            Title = dto.Title,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Suffix = dto.Suffix,
            EmailPromotion = dto.EmailPromotion,
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.Now
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync(ct);

        dto.BusinessEntityID = person.BusinessEntityID;
        return dto;
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        var person = await _context.People.FindAsync(new object[] { id }, ct);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {id} not found");

        person.PersonType = dto.PersonType;
        person.Title = dto.Title;
        person.FirstName = dto.FirstName;
        person.MiddleName = dto.MiddleName;
        person.LastName = dto.LastName;
        person.Suffix = dto.Suffix;
        person.EmailPromotion = dto.EmailPromotion;
        person.ModifiedDate = DateTime.Now;

        await _context.SaveChangesAsync(ct);

        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var person = await _context.People.FindAsync(new object[] { id }, ct);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {id} not found");

        _context.People.Remove(person);
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
                          ProductID       = p.ProductID,
                          Name            = p.Name,
                          ProductNumber   = p.ProductNumber,
                          Color           = p.Color,
                          ListPrice       = p.ListPrice,
                          SafetyStockLevel = p.SafetyStockLevel,
                          CategoryName    = c != null ? c.Name : null,
                          SubcategoryName = sc != null ? sc.Name : null
                      }).ToListAsync(ct);
    }

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        var query = from p in _context.Products
                    join sc in _context.ProductSubcategories
                        on p.ProductSubcategoryID equals sc.ProductSubcategoryID into scGroup
                    from sc in scGroup.DefaultIfEmpty()
                    join c in _context.ProductCategories
                        on sc.ProductCategoryID equals c.ProductCategoryID into cGroup
                    from c in cGroup.DefaultIfEmpty()
                    select new { p, sc, c };

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(x => x.c != null && x.c.Name.Contains(categoryName));

        return await query.Select(x => new ProductDto
        {
            ProductID = x.p.ProductID,
            Name = x.p.Name,
            ProductNumber = x.p.ProductNumber,
            Color = x.p.Color,
            ListPrice = x.p.ListPrice,
            SafetyStockLevel = x.p.SafetyStockLevel,
            CategoryName = x.c != null ? x.c.Name : null,
            SubcategoryName = x.sc != null ? x.sc.Name : null
        }).ToListAsync(ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var product = new Domain.AdventureWorks.Entities.Product
        {
            Name = dto.Name,
            ProductNumber = dto.ProductNumber,
            Color = dto.Color,
            ListPrice = dto.ListPrice,
            MakeFlag = true,
            FinishedGoodsFlag = true,
            SafetyStockLevel = 1,
            ReorderPoint = 1,
            StandardCost = 0,
            DaysToManufacture = 0,
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.Now
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);

        dto.ProductID = product.ProductID;
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, ct);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        product.Name = dto.Name;
        product.ProductNumber = dto.ProductNumber;
        product.Color = dto.Color;
        product.ListPrice = dto.ListPrice;
        product.ModifiedDate = DateTime.Now;

        await _context.SaveChangesAsync(ct);

        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, ct);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }
}