
namespace CodingBasics.Domain.Contracts;

/// <summary>
/// Service contract for Person operations.
/// TODO (Workshop): Add Create, Update, Delete methods for full CRUD support
/// </summary>
public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default);
    Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Service contract for Product operations.
/// TODO (Workshop): Add Create, Update, Delete methods for full CRUD support
/// </summary>
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Person data access.
/// Implementation should use EF Core with AdventureWorksDbContext.
/// TODO (Workshop): Add Create, Update, Delete methods for full CRUD support
/// </summary>
public interface IPersonRepository
{
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default);
    Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Product data access.
/// Implementation should use EF Core with AdventureWorksDbContext.
/// Note: CategoryName requires joining Product -> ProductSubcategory -> ProductCategory.
/// TODO (Workshop): Add Create, Update, Delete methods for full CRUD support
/// </summary>
public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}