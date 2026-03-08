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
        // Implement search with optional filters
        
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
                PersonType = p.PersonType 
            }).ToListAsync(ct);
    
    }

    // TODO (Workshop): Implement CreateAsync, UpdateAsync, DeleteAsync for Person

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        // Nota: Aquí se implementaría la lógica de inserción real en la DB
        return dto; 
    }

    public async Task<PersonDto> UpdateAsync(PersonDto dto, CancellationToken ct = default)
    {
        // Nota: Aquí se implementaría la lógica de actualización real en la DB
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        // Nota: Aquí se implementaría el borrado real por ID
        await Task.CompletedTask;
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
                           CategoryName    = c != null ? c.Name : null,
                           SubcategoryName = sc != null ? sc.Name : null
                       }).ToListAsync(ct);
       
    }

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
    // 1. Creamos la consulta base con los Joins (igual que en GetAllAsync)
    var query = from p in _context.Products
                join sc in _context.ProductSubcategories on p.ProductSubcategoryID equals sc.ProductSubcategoryID into scGroup
                from sc in scGroup.DefaultIfEmpty()
                join c in _context.ProductCategories on sc.ProductCategoryID equals c.ProductCategoryID into cGroup
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

    // 2. Aplicamos filtros opcionales (solo si el usuario escribió algo)
    if (!string.IsNullOrWhiteSpace(name))
    {
        query = query.Where(p => p.Name.Contains(name));
    }

    if (!string.IsNullOrWhiteSpace(categoryName))
    {
        query = query.Where(p => p.CategoryName == categoryName);
    }

    // 3. Ejecutamos la consulta y devolvemos la lista
    return await query.ToListAsync(ct);
    }

public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        // En un taller real aquí usaríamos _context.Products.Add(...)
        return await Task.FromResult(dto);
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        // Aquí recibimos el ID y el objeto como pide la interfaz
        return await Task.FromResult(dto);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        // Simulamos la eliminación exitosa
        await Task.CompletedTask;
    }

}   