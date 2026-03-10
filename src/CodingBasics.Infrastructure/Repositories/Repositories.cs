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
        return await _context.People
            .Select(p => new PersonDto
            {
                BusinessEntityID = p.BusinessEntityID,
                PersonType       = p.PersonType,
                Title            = p.Title,
                FirstName        = p.FirstName,
                MiddleName       = p.MiddleName,
                LastName         = p.LastName,
                Suffix           = p.Suffix,
                EmailPromotion   = p.EmailPromotion
            })
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        var query = _context.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));
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

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        // Person.BusinessEntityID is a FK to Person.BusinessEntity (NOT IDENTITY on Person.Person).
        // Insert a BusinessEntity row first so the DB generates the ID via its own IDENTITY column,
        // then assign that ID to the new Person.
        var businessEntity = new Domain.AdventureWorks.Entities.BusinessEntity
        {
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.UtcNow
        };

        _context.BusinessEntities.Add(businessEntity);
        await _context.SaveChangesAsync(ct);

        var entity = new Domain.AdventureWorks.Entities.Person
        {
            BusinessEntityID = businessEntity.BusinessEntityID,
            PersonType = dto.PersonType,
            Title = dto.Title,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Suffix = dto.Suffix,
            EmailPromotion = dto.EmailPromotion,
            NameStyle = false,
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.UtcNow
        };

        _context.People.Add(entity);
        await _context.SaveChangesAsync(ct);

        dto.BusinessEntityID = entity.BusinessEntityID;
        return dto;
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        // ExecuteUpdateAsync generates a direct SQL UPDATE with only the target columns.
        // This avoids EF Core change-tracking loading the full Person row (which includes
        // xml-typed columns AdditionalContactInfo / Demographics that can cause tracking
        // comparison issues and trigger unexpected SET clauses).
        var affected = await _context.People
            .Where(p => p.BusinessEntityID == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.PersonType,     dto.PersonType)
                .SetProperty(p => p.Title,           dto.Title)
                .SetProperty(p => p.FirstName,       dto.FirstName)
                .SetProperty(p => p.MiddleName,      dto.MiddleName)
                .SetProperty(p => p.LastName,        dto.LastName)
                .SetProperty(p => p.Suffix,          dto.Suffix)
                .SetProperty(p => p.EmailPromotion,  dto.EmailPromotion)
                .SetProperty(p => p.ModifiedDate,    DateTime.UtcNow),
            ct);

        if (affected == 0)
            throw new KeyNotFoundException($"Person with ID {id} was not found.");

        return new PersonDto
        {
            BusinessEntityID = id,
            PersonType       = dto.PersonType,
            Title            = dto.Title,
            FirstName        = dto.FirstName,
            MiddleName       = dto.MiddleName,
            LastName         = dto.LastName,
            Suffix           = dto.Suffix,
            EmailPromotion   = dto.EmailPromotion
        };
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.People.FirstOrDefaultAsync(p => p.BusinessEntityID == id, ct);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Person with ID {id} was not found.");
        }

        _context.People.Remove(entity);
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
                          ProductID = p.ProductID,
                          Name = p.Name,
                          ProductNumber = p.ProductNumber,
                          Color = p.Color,
                          ListPrice = p.ListPrice,
                          CategoryName = c != null ? c.Name : null,
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
                    select new
                    {
                        Product = p,
                        Subcategory = sc,
                        Category = c
                    };

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(x => x.Product.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            query = query.Where(x => x.Category != null && x.Category.Name.Contains(categoryName));
        }

        return await query
            .Select(x => new ProductDto
            {
                ProductID = x.Product.ProductID,
                Name = x.Product.Name,
                ProductNumber = x.Product.ProductNumber,
                Color = x.Product.Color,
                ListPrice = x.Product.ListPrice,
                CategoryName = x.Category != null ? x.Category.Name : null,
                SubcategoryName = x.Subcategory != null ? x.Subcategory.Name : null
            })
            .ToListAsync(ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var productNumber = string.IsNullOrWhiteSpace(dto.ProductNumber)
            ? $"WS-{Guid.NewGuid():N}"[..12]
            : dto.ProductNumber;

        var entity = new Domain.AdventureWorks.Entities.Product
        {
            Name = string.IsNullOrWhiteSpace(dto.Name) ? "Workshop Product" : dto.Name,
            ProductNumber = productNumber,
            Color = dto.Color,
            ListPrice = dto.ListPrice,
            ProductSubcategoryID = null, // TODO: Map from SubcategoryName if provided
            MakeFlag = false,
            FinishedGoodsFlag = false,
            SafetyStockLevel = 1,
            ReorderPoint = 1,
            StandardCost = 0,
            DaysToManufacture = 0,
            SellStartDate = now,
            rowguid = Guid.NewGuid(),
            ModifiedDate = now
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync(ct);

        dto.ProductID = entity.ProductID;
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        var affected = await _context.Products
            .Where(p => p.ProductID == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name,          dto.Name)
                .SetProperty(p => p.ProductNumber,  dto.ProductNumber)
                .SetProperty(p => p.Color,          dto.Color)
                .SetProperty(p => p.ListPrice,      dto.ListPrice)
                .SetProperty(p => p.ModifiedDate,   DateTime.UtcNow),
            ct);

        if (affected == 0)
            throw new KeyNotFoundException($"Product with ID {id} was not found.");

        dto.ProductID = id;
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id, ct);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Product with ID {id} was not found.");
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}