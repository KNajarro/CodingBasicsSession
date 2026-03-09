using System.Threading;
using System.Threading.Tasks;

namespace CodingBasics.Domain.Contracts;

/// <summary>
/// Service contract for Person operations.
/// TODO (Workshop): Add Create, Update, Delete methods for full CRUD support
/// </summary>
public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default);
    // TODO: Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    // TODO: Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    // TODO: Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// Service contract for Product operations.
/// TODO (Workshop): Add Create, Update, Delete methods for full CRUD support
/// </summary>
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default);
    // TODO: Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    // TODO: Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    // TODO: Task DeleteAsync(int id, CancellationToken ct = default);
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
    // TODO: Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
    // TODO: Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
    // TODO: Task DeleteAsync(int id, CancellationToken ct = default);
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
    // TODO: Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
    // TODO: Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
    // TODO: Task DeleteAsync(int id, CancellationToken ct = default);
}

//Agregado para el funcionamiento del Dashboard//
/// <summary>
/// Service contract for Dashboard operations.
/// </summary>
public interface IDashboardService
{
    Task<decimal> GetInventoryValueAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetProductsByColorAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetPeopleByTypeAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetLowStockProductsAsync(CancellationToken ct = default);
}

/// <summary>
/// Repository contract for Dashboard queries.
/// </summary>
public interface IDashboardRepository
{
    Task<decimal> GetInventoryValueAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetProductsByColorAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetPeopleByTypeAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetLowStockProductsAsync(CancellationToken ct = default);
}