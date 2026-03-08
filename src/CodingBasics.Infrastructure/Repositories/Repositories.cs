using Microsoft.EntityFrameworkCore;
using CodingBasics.Domain.Contracts;
using CodingBasics.Infrastructure.Persistence;
using CodingBasics.Domain.AdventureWorks.Entities;

namespace CodingBasics.Infrastructure.Repositories;

/// <summary>
/// Person repository implementation using EF Core.
/// Provides full CRUD operations for Person entities.
/// </summary>
public sealed class PersonRepository : IPersonRepository
{
    private readonly AdventureWorksDbContext _context;

    public PersonRepository(AdventureWorksDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PersonDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.People
            .Select(p => new PersonDto
            {
                BusinessEntityID = p.BusinessEntityID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PersonType = p.PersonType
            });
        return await PaginatedList<PersonDto>.CreateAsync(query, page, pageSize, ct);
    }

    public async Task<PaginatedList<PersonDto>> SearchAsync(string filter, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter) || p.PersonType.Contains(filter));

        var projected = query.Select(p => new PersonDto
        {
            BusinessEntityID = p.BusinessEntityID,
            FirstName = p.FirstName,
            LastName = p.LastName,
            PersonType = p.PersonType
        });

        return await PaginatedList<PersonDto>.CreateAsync(projected, page, pageSize, ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        // basic validation to avoid blowing past the SQL CHECK
        if (!string.IsNullOrEmpty(dto.PersonType) && dto.PersonType.Length > 2)
        {
            throw new ArgumentException("PersonType must be at most 2 characters.", nameof(dto.PersonType));
        }

        // obtain next ID from BusinessEntity table and insert there first
        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync(ct);

        int newId;
        // insert into BusinessEntity and let identity generate the key
        using (var beCmd = conn.CreateCommand())
        {
            beCmd.CommandText = @"INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate)
                                     VALUES (@Rowguid, @ModifiedDate);
                                 SELECT CAST(SCOPE_IDENTITY() as int);";
            var p = beCmd.CreateParameter(); p.ParameterName = "@Rowguid"; p.Value = Guid.NewGuid(); beCmd.Parameters.Add(p);
            p = beCmd.CreateParameter(); p.ParameterName = "@ModifiedDate"; p.Value = DateTime.UtcNow; beCmd.Parameters.Add(p);
            var result = await beCmd.ExecuteScalarAsync(ct);
            newId = Convert.ToInt32(result);
        }

        // now insert person row
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Person.Person
            (BusinessEntityID, PersonType, Title, FirstName, MiddleName, LastName, Suffix, EmailPromotion, ModifiedDate, rowguid)
            VALUES
            (@Id, @PersonType, @Title, @FirstName, @MiddleName, @LastName, @Suffix, @EmailPromotion, @ModifiedDate, @rowguid);";

        var p2 = cmd.CreateParameter(); p2.ParameterName = "@Id"; p2.Value = newId; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@PersonType"; p2.Value = (object)dto.PersonType ?? DBNull.Value; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@Title"; p2.Value = (object)dto.Title ?? DBNull.Value; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@FirstName"; p2.Value = dto.FirstName; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@MiddleName"; p2.Value = (object)dto.MiddleName ?? DBNull.Value; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@LastName"; p2.Value = dto.LastName; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@Suffix"; p2.Value = (object)dto.Suffix ?? DBNull.Value; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@EmailPromotion"; p2.Value = dto.EmailPromotion; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@ModifiedDate"; p2.Value = DateTime.UtcNow; cmd.Parameters.Add(p2);
        p2 = cmd.CreateParameter(); p2.ParameterName = "@rowguid"; p2.Value = Guid.NewGuid(); cmd.Parameters.Add(p2);

        try
        {
            await cmd.ExecuteNonQueryAsync(ct);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 547)
        {
            // translate foreign-key/check-constraint violations to user-friendly message
            throw new ArgumentException("PersonType value is invalid or violates database constraints.", nameof(dto.PersonType));
        }

        dto.BusinessEntityID = newId;
        return dto;
    }

    public async Task<PersonDto> UpdateAsync(PersonDto dto, CancellationToken ct = default)
    {
        if (!string.IsNullOrEmpty(dto.PersonType) && dto.PersonType.Length > 2)
        {
            throw new ArgumentException("PersonType must be at most 2 characters.", nameof(dto.PersonType));
        }

        var entity = await _context.People.FindAsync([dto.BusinessEntityID], ct);
        if (entity == null) throw new KeyNotFoundException($"Person with ID {dto.BusinessEntityID} not found.");

        entity.PersonType = dto.PersonType;
        entity.Title = dto.Title;
        entity.FirstName = dto.FirstName;
        entity.MiddleName = dto.MiddleName;
        entity.LastName = dto.LastName;
        entity.Suffix = dto.Suffix;
        entity.EmailPromotion = dto.EmailPromotion;
        entity.ModifiedDate = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 547)
        {
            throw new ArgumentException("PersonType value is invalid or violates database constraints.", nameof(dto.PersonType));
        }

        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.People.FindAsync([id], ct);
        if (entity == null) throw new KeyNotFoundException($"Person with ID {id} not found.");

        _context.People.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}

/// <summary>
/// Product repository implementation using EF Core.
/// Provides query methods with category joins for full CRUD operations.
/// </summary>
public sealed class ProductRepository : IProductRepository
{
    private readonly AdventureWorksDbContext _context;

    public ProductRepository(AdventureWorksDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ProductDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
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
                        CategoryName = c != null ? c.Name : null,
                        SubcategoryName = sc != null ? sc.Name : null
                    };
        return await PaginatedList<ProductDto>.CreateAsync(query, page, pageSize, ct);
    }

    public async Task<PaginatedList<ProductDto>> SearchAsync(string filter, int page, int pageSize, CancellationToken ct = default)
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
                        CategoryName = c != null ? c.Name : null,
                        SubcategoryName = sc != null ? sc.Name : null
                    };

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(p => p.Name.Contains(filter) || (p.CategoryName != null && p.CategoryName.Contains(filter)));
        }

        return await PaginatedList<ProductDto>.CreateAsync(query, page, pageSize, ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        // Raw SQL insert to avoid OUTPUT clause when triggers are present
        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync(ct);

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Production.Product
            (Name, ProductNumber, Color, ListPrice, ModifiedDate, rowguid)
            VALUES
            (@Name, @ProductNumber, @Color, @ListPrice, @ModifiedDate, @rowguid);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        var p = cmd.CreateParameter(); p.ParameterName = "@Name"; p.Value = dto.Name; cmd.Parameters.Add(p);
        p = cmd.CreateParameter(); p.ParameterName = "@ProductNumber"; p.Value = dto.ProductNumber; cmd.Parameters.Add(p);
        p = cmd.CreateParameter(); p.ParameterName = "@Color"; p.Value = (object)dto.Color ?? DBNull.Value; cmd.Parameters.Add(p);
        p = cmd.CreateParameter(); p.ParameterName = "@ListPrice"; p.Value = dto.ListPrice; cmd.Parameters.Add(p);
        p = cmd.CreateParameter(); p.ParameterName = "@ModifiedDate"; p.Value = DateTime.UtcNow; cmd.Parameters.Add(p);
        p = cmd.CreateParameter(); p.ParameterName = "@rowguid"; p.Value = Guid.NewGuid(); cmd.Parameters.Add(p);

        var result = await cmd.ExecuteScalarAsync(ct);
        dto.ProductID = (result is int i) ? i : Convert.ToInt32(result);
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var entity = await _context.Products.FindAsync([dto.ProductID], ct);
        if (entity == null) throw new KeyNotFoundException($"Product with ID {dto.ProductID} not found.");

        entity.Name = dto.Name;
        entity.ProductNumber = dto.ProductNumber;
        entity.Color = dto.Color;
        entity.ListPrice = dto.ListPrice;
        entity.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.Products.FindAsync([id], ct);
        if (entity == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}