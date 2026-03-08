using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
using CodingBasics.Infrastructure.Persistence;
using CodingBasics.Domain.AdventureWorks.Entities;

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
            PersonType = p.PersonType,
            NameStyle = p.NameStyle,
            Title = p.Title,
            FirstName = p.FirstName,
            MiddleName = p.MiddleName,
            LastName = p.LastName,
            Suffix = p.Suffix,
            EmailPromotion = p.EmailPromotion,
            AdditionalContactInfo = p.AdditionalContactInfo,
            
            })
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        // TODO (Workshop): Implement search with optional filters
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
            NameStyle = p.NameStyle,
            Title = p.Title,
            FirstName = p.FirstName,
            MiddleName = p.MiddleName,
            LastName = p.LastName,
            Suffix = p.Suffix,
            EmailPromotion = p.EmailPromotion,
            AdditionalContactInfo = p.AdditionalContactInfo,
            
        })
        .ToListAsync(ct);

        
    }

    // TODO (Workshop): Implement CreateAsync, UpdateAsync, DeleteAsync for Person
    /*
    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
{
    var person = new Person
    {
        BusinessEntityID = dto.BusinessEntityID,
        PersonType = dto.PersonType,
        NameStyle = dto.NameStyle,
        Title = dto.Title,
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        LastName = dto.LastName,
        Suffix = dto.Suffix,
        EmailPromotion = dto.EmailPromotion,
        AdditionalContactInfo = dto.AdditionalContactInfo,
        
    };

    person.rowguid = Guid.NewGuid();
person.ModifiedDate = DateTime.Now;

    await _context.People.AddAsync(person, ct);
    await _context.SaveChangesAsync(ct);

    return new PersonDto
    {
        BusinessEntityID = person.BusinessEntityID,
        PersonType = person.PersonType,
        NameStyle = person.NameStyle,
        Title = person.Title,
        FirstName = person.FirstName,
        MiddleName = person.MiddleName,
        LastName = person.LastName,
        Suffix = person.Suffix,
        EmailPromotion = person.EmailPromotion,
        AdditionalContactInfo = person.AdditionalContactInfo,
        
    };
}
*/
public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
{
    var person = await _context.People.FirstOrDefaultAsync(p => p.BusinessEntityID == id, ct);

    if (person == null)
        throw new KeyNotFoundException($"Person with id {id} was not found.");

    person.PersonType = dto.PersonType;
    person.NameStyle = dto.NameStyle;
    person.Title = dto.Title;
    person.FirstName = dto.FirstName;
    person.MiddleName = dto.MiddleName;
    person.LastName = dto.LastName;
    person.Suffix = dto.Suffix;
    person.EmailPromotion = dto.EmailPromotion;
    person.AdditionalContactInfo = dto.AdditionalContactInfo;
    person.ModifiedDate = DateTime.Now;
    

    await _context.SaveChangesAsync(ct);

    return new PersonDto
    {
        BusinessEntityID = person.BusinessEntityID,
        PersonType = person.PersonType,
        NameStyle = person.NameStyle,
        Title = person.Title,
        FirstName = person.FirstName,
        MiddleName = person.MiddleName,
        LastName = person.LastName,
        Suffix = person.Suffix,
        EmailPromotion = person.EmailPromotion,
        AdditionalContactInfo = person.AdditionalContactInfo,
        
    };
}

public async Task DeleteAsync(int id, CancellationToken ct = default)
{
    var person = await _context.People.FirstOrDefaultAsync(p => p.BusinessEntityID == id, ct);

    if (person == null)
        throw new KeyNotFoundException($"Person with id {id} was not found.");

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

        // TODO (Workshop): Query products with category information
        // Hint: Left-outer join Product -> ProductSubcategory -> ProductCategory
        //
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
                          MakeFlag = p.MakeFlag,
                          FinishedGoodsFlag = p.FinishedGoodsFlag,
                          SafetyStockLevel = p.SafetyStockLevel,
                          ReorderPoint = p.ReorderPoint,
                          StandardCost = p.StandardCost,
                          Size = p.Size,
                          SizeUnitMeasureCode = p.SizeUnitMeasureCode,
                          WeightUnitMeasureCode = p.WeightUnitMeasureCode,
                          Weight = p.Weight,
                          DaysToManufacture = p.DaysToManufacture,
                          ProductLine = p.ProductLine,
                          Class = p.Class,
                          Style = p.Style,
                          ProductSubcategoryID = p.ProductSubcategoryID,
                          ProductModelID = p.ProductModelID,
                          SellStartDate = p.SellStartDate,
                          SellEndDate = p.SellEndDate,
                          DiscontinuedDate = p.DiscontinuedDate,
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
                    select new ProductDto
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        ProductNumber = p.ProductNumber,
                        Color = p.Color,
                        ListPrice = p.ListPrice,
                        MakeFlag = p.MakeFlag,
                        FinishedGoodsFlag = p.FinishedGoodsFlag,
                        SafetyStockLevel = p.SafetyStockLevel,
                        ReorderPoint = p.ReorderPoint,
                        StandardCost = p.StandardCost,
                        Size = p.Size,
                        SizeUnitMeasureCode = p.SizeUnitMeasureCode,
                        WeightUnitMeasureCode = p.WeightUnitMeasureCode,
                        Weight = p.Weight,
                        DaysToManufacture = p.DaysToManufacture,
                        ProductLine = p.ProductLine,
                        Class = p.Class,
                        Style = p.Style,
                        ProductSubcategoryID = p.ProductSubcategoryID,
                        ProductModelID = p.ProductModelID,
                        SellStartDate = p.SellStartDate,
                        SellEndDate = p.SellEndDate,
                        DiscontinuedDate = p.DiscontinuedDate,
                        CategoryName = c != null ? c.Name : null,
                        SubcategoryName = sc != null ? sc.Name : null
                    };

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(p => p.CategoryName != null && p.CategoryName.Contains(categoryName));

        return await query.ToListAsync(ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var product = new Product
        {
            ProductID = dto.ProductID,
            Name = dto.Name,
            ProductNumber = dto.ProductNumber,
            MakeFlag = dto.MakeFlag,
            FinishedGoodsFlag = dto.FinishedGoodsFlag,
            Color = dto.Color,
            SafetyStockLevel = dto.SafetyStockLevel,
            ReorderPoint = dto.ReorderPoint,
            StandardCost = dto.StandardCost,
            ListPrice = dto.ListPrice,
            Size = dto.Size,
            SizeUnitMeasureCode = dto.SizeUnitMeasureCode,
            WeightUnitMeasureCode = dto.WeightUnitMeasureCode,
            Weight = dto.Weight,
            DaysToManufacture = dto.DaysToManufacture,
            ProductLine = dto.ProductLine,
            Class = dto.Class,
            Style = dto.Style,
            ProductSubcategoryID = dto.ProductSubcategoryID,
            ProductModelID = dto.ProductModelID,
            SellStartDate = dto.SellStartDate,
            SellEndDate = dto.SellEndDate,
            DiscontinuedDate = dto.DiscontinuedDate
        };

        product.rowguid = Guid.NewGuid();
product.ModifiedDate = DateTime.Now;

        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);

        string? subcategoryName = null;
        string? categoryNameValue = null;

        if (product.ProductSubcategoryID.HasValue)
        {
            var subcategory = await _context.ProductSubcategories
                .FirstOrDefaultAsync(sc => sc.ProductSubcategoryID == product.ProductSubcategoryID.Value, ct);

            if (subcategory != null)
            {
                subcategoryName = subcategory.Name;

                var category = await _context.ProductCategories
                    .FirstOrDefaultAsync(c => c.ProductCategoryID == subcategory.ProductCategoryID, ct);

                categoryNameValue = category?.Name;
            }
        }

        return new ProductDto
        {
            ProductID = product.ProductID,
            Name = product.Name,
            ProductNumber = product.ProductNumber,
            MakeFlag = product.MakeFlag,
            FinishedGoodsFlag = product.FinishedGoodsFlag,
            Color = product.Color,
            SafetyStockLevel = product.SafetyStockLevel,
            ReorderPoint = product.ReorderPoint,
            StandardCost = product.StandardCost,
            ListPrice = product.ListPrice,
            Size = product.Size,
            SizeUnitMeasureCode = product.SizeUnitMeasureCode,
            WeightUnitMeasureCode = product.WeightUnitMeasureCode,
            Weight = product.Weight,
            DaysToManufacture = product.DaysToManufacture,
            ProductLine = product.ProductLine,
            Class = product.Class,
            Style = product.Style,
            ProductSubcategoryID = product.ProductSubcategoryID,
            ProductModelID = product.ProductModelID,
            SellStartDate = product.SellStartDate,
            SellEndDate = product.SellEndDate,
            DiscontinuedDate = product.DiscontinuedDate,
            CategoryName = categoryNameValue,
            SubcategoryName = subcategoryName
        };
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id, ct);

        if (product == null)
            throw new KeyNotFoundException($"Product with id {id} was not found.");

        product.Name = dto.Name;
        product.ProductNumber = dto.ProductNumber;
        product.MakeFlag = dto.MakeFlag;
        product.FinishedGoodsFlag = dto.FinishedGoodsFlag;
        product.Color = dto.Color;
        product.SafetyStockLevel = dto.SafetyStockLevel;
        product.ReorderPoint = dto.ReorderPoint;
        product.StandardCost = dto.StandardCost;
        product.ListPrice = dto.ListPrice;
        product.Size = dto.Size;
        product.SizeUnitMeasureCode = dto.SizeUnitMeasureCode;
        product.WeightUnitMeasureCode = dto.WeightUnitMeasureCode;
        product.Weight = dto.Weight;
        product.DaysToManufacture = dto.DaysToManufacture;
        product.ProductLine = dto.ProductLine;
        product.Class = dto.Class;
        product.Style = dto.Style;
        product.ProductSubcategoryID = dto.ProductSubcategoryID;
        product.ProductModelID = dto.ProductModelID;
        product.SellStartDate = dto.SellStartDate;
        product.SellEndDate = dto.SellEndDate;
        product.DiscontinuedDate = dto.DiscontinuedDate;
        product.ModifiedDate = DateTime.Now;
        

        await _context.SaveChangesAsync(ct);

        string? subcategoryName = null;
        string? categoryNameValue = null;

        if (product.ProductSubcategoryID.HasValue)
        {
            var subcategory = await _context.ProductSubcategories
                .FirstOrDefaultAsync(sc => sc.ProductSubcategoryID == product.ProductSubcategoryID.Value, ct);

            if (subcategory != null)
            {
                subcategoryName = subcategory.Name;

                var category = await _context.ProductCategories
                    .FirstOrDefaultAsync(c => c.ProductCategoryID == subcategory.ProductCategoryID, ct);

                categoryNameValue = category?.Name;
            }
        }

        return new ProductDto
        {
            ProductID = product.ProductID,
            Name = product.Name,
            ProductNumber = product.ProductNumber,
            MakeFlag = product.MakeFlag,
            FinishedGoodsFlag = product.FinishedGoodsFlag,
            Color = product.Color,
            SafetyStockLevel = product.SafetyStockLevel,
            ReorderPoint = product.ReorderPoint,
            StandardCost = product.StandardCost,
            ListPrice = product.ListPrice,
            Size = product.Size,
            SizeUnitMeasureCode = product.SizeUnitMeasureCode,
            WeightUnitMeasureCode = product.WeightUnitMeasureCode,
            Weight = product.Weight,
            DaysToManufacture = product.DaysToManufacture,
            ProductLine = product.ProductLine,
            Class = product.Class,
            Style = product.Style,
            ProductSubcategoryID = product.ProductSubcategoryID,
            ProductModelID = product.ProductModelID,
            SellStartDate = product.SellStartDate,
            SellEndDate = product.SellEndDate,
            DiscontinuedDate = product.DiscontinuedDate,
            CategoryName = categoryNameValue,
            SubcategoryName = subcategoryName
        };
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id, ct);

        if (product == null)
            throw new KeyNotFoundException($"Product with id {id} was not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }
}