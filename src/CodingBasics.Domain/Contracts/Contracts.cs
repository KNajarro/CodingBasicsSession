using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CodingBasics.Domain.Contracts;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
public class PaginatedList<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize, CancellationToken ct = default)
    {
        var totalCount = await source.CountAsync(ct);
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PaginatedList<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}

/// <summary>
/// Service contract for Person operations.
/// </summary>
public interface IPersonService
{
    Task<PaginatedList<PersonDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PaginatedList<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default);
    Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Service contract for Product operations.
/// </summary>
public interface IProductService
{
    Task<PaginatedList<ProductDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PaginatedList<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Person data access.
/// Implementation should use EF Core with AdventureWorksDbContext.
/// </summary>
public interface IPersonRepository
{
    Task<PaginatedList<PersonDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PaginatedList<PersonDto>> SearchAsync(string filter, int page, int pageSize, CancellationToken ct = default);
    Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    Task<PersonDto> UpdateAsync(PersonDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Product data access.
/// Implementation should use EF Core with AdventureWorksDbContext.
/// Note: CategoryName requires joining Product -> ProductSubcategory -> ProductCategory.
/// </summary>
public interface IProductRepository
{
    Task<PaginatedList<ProductDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PaginatedList<ProductDto>> SearchAsync(string filter, int page, int pageSize, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(ProductDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}