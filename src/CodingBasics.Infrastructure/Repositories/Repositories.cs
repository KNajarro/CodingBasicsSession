using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
using CodingBasics.Domain.AdventureWorks.Entities;
using CodingBasics.Infrastructure.Persistence;

namespace CodingBasics.Infrastructure.Repositories;

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

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        var query = _context.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));

        if (!string.IsNullOrWhiteSpace(personType))
            query = query.Where(p => p.PersonType == personType);

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

    public async Task<PersonDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.People
            .Where(p => p.BusinessEntityID == id)
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
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        var entity = new Person
        {
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
        var entity = await _context.People.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Person with ID {id} not found.");

        entity.PersonType = dto.PersonType;
        entity.Title = dto.Title;
        entity.FirstName = dto.FirstName;
        entity.MiddleName = dto.MiddleName;
        entity.LastName = dto.LastName;
        entity.Suffix = dto.Suffix;
        entity.EmailPromotion = dto.EmailPromotion;
        entity.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        dto.BusinessEntityID = entity.BusinessEntityID;
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.People.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Person with ID {id} not found.");

        _context.People.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}

public sealed class ProductRepository : IProductRepository
{
    private readonly AdventureWorksDbContext _context;

    public ProductRepository(AdventureWorksDbContext context)
    {
        _context = context;
    }

    private IQueryable<ProductDto> BuildProductQuery()
    {
        return from p in _context.Products
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
                   CategoryName    = c != null ? c.Name : null,
                   SubcategoryName = sc != null ? sc.Name : null
               };
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await BuildProductQuery().ToListAsync(ct);
    }

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        var query = BuildProductQuery();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(p => p.CategoryName == categoryName);

        return await query.ToListAsync(ct);
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await BuildProductQuery()
            .FirstOrDefaultAsync(p => p.ProductID == id, ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var entity = new CodingBasics.Domain.AdventureWorks.Entities.Product
        {
            Name = dto.Name,
            ProductNumber = dto.ProductNumber,
            Color = dto.Color,
            ListPrice = dto.ListPrice,
            StandardCost = 0,
            SafetyStockLevel = 0,
            ReorderPoint = 0,
            DaysToManufacture = 0,
            MakeFlag = false,
            FinishedGoodsFlag = false,
            SellStartDate = DateTime.UtcNow,
            rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.UtcNow
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync(ct);

        dto.ProductID = entity.ProductID;
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        var entity = await _context.Products.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

        entity.Name = dto.Name;
        entity.ProductNumber = dto.ProductNumber;
        entity.Color = dto.Color;
        entity.ListPrice = dto.ListPrice;
        entity.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        dto.ProductID = entity.ProductID;
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.Products.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}