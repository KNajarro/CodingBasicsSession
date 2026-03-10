using System.Threading;
using System.Threading.Tasks;

namespace CodingBasics.Domain.Contracts;

/// <summary>
/// Service contract for Person operations.
/// </summary>
public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default);
    Task<PersonDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<string>> GetPersonTypesAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonTypeCountDto>> GetPersonTypeDistributionAsync(CancellationToken ct = default);
}

/// <summary>
/// Service contract for Product operations.
/// </summary>
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<string>> GetProductCategoriesAsync(CancellationToken ct = default);
    Task<decimal> GetInventoryValueAsync(CancellationToken ct = default);
    Task<IEnumerable<ColorCountDto>> GetColorDistributionAsync(CancellationToken ct = default);
    Task<PagedResult<LowStockProductDto>> GetLowStockAsync(int page, short threshold, CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Person data access.
/// Implementation should use EF Core with AdventureWorksDbContext.
/// </summary>
public interface IPersonRepository
{
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default);
    Task<PersonDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<string>> GetPersonTypesAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonTypeCountDto>> GetPersonTypeDistributionAsync(CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Product data access.
/// Implementation should use EF Core with AdventureWorksDbContext.
/// Note: CategoryName requires joining Product -> ProductSubcategory -> ProductCategory.
/// </summary>
public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<string>> GetProductCategoriesAsync(CancellationToken ct = default);
    Task<decimal> GetInventoryValueAsync(CancellationToken ct = default);
    Task<IEnumerable<ColorCountDto>> GetColorDistributionAsync(CancellationToken ct = default);
    Task<PagedResult<LowStockProductDto>> GetLowStockAsync(int page, short threshold, CancellationToken ct = default);
}