using CodingBasics.Domain.Contracts;
using CodingBasics.Domain.AdventureWorks.Entities;

namespace CodingBasics.Application.Services;

/// <summary>
/// Person service implementation.
/// </summary>
public sealed class PersonService : IPersonService
{
  private readonly IPersonRepository _repository;

  public PersonService(IPersonRepository repository)
  {
    _repository = repository;
  }

  public async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default)
  {
    var persons = await _repository.GetAllAsync(ct);
    return persons.Select(MapToDto);
  }

  public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
  {
    var persons = await _repository.SearchAsync(name, personType, ct);
    return persons.Select(MapToDto);
  }

  public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
  {
    var entity = MapToEntity(dto);
    var created = await _repository.CreateAsync(entity, ct);
    return MapToDto(created);
  }

  public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
  {
    var entity = MapToEntity(dto);
    var updated = await _repository.UpdateAsync(id, entity, ct);
    return MapToDto(updated);
  }

  public Task DeleteAsync(int id, CancellationToken ct = default)
  {
    return _repository.DeleteAsync(id, ct);
  }

  private static PersonDto MapToDto(Person entity) => new()
  {
    BusinessEntityID = entity.BusinessEntityID,
    PersonType = entity.PersonType,
    Title = entity.Title,
    FirstName = entity.FirstName,
    MiddleName = entity.MiddleName,
    LastName = entity.LastName,
    Suffix = entity.Suffix,
    EmailPromotion = entity.EmailPromotion
  };

  private static Person MapToEntity(PersonDto dto) => new()
  {
    BusinessEntityID = dto.BusinessEntityID,
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
}

/// <summary>
/// Product service implementation.
/// </summary>
public sealed class ProductService : IProductService
{
  private readonly IProductRepository _repository;

  public ProductService(IProductRepository repository)
  {
    _repository = repository;
  }

  public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
  {
    var products = await _repository.GetAllAsync(ct);
    return products.Select(MapToDto);
  }

  public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
  {
    var products = await _repository.SearchAsync(name, categoryName, ct);
    return products.Select(MapToDto);
  }

  public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
  {
    var entity = MapToEntity(dto);
    var created = await _repository.CreateAsync(entity, ct);
    return MapToDto(created);
  }

  public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
  {
    var entity = MapToEntity(dto);
    var updated = await _repository.UpdateAsync(id, entity, ct);
    return MapToDto(updated);
  }

  public Task DeleteAsync(int id, CancellationToken ct = default)
  {
    return _repository.DeleteAsync(id, ct);
  }

  private static ProductDto MapToDto(Product entity) => new()
  {
    ProductID = entity.ProductID,
    Name = entity.Name,
    ProductNumber = entity.ProductNumber,
    Color = entity.Color,
    ListPrice = entity.ListPrice,
    CategoryName = null, // Set by repository with joins
    SubcategoryName = null // Set by repository with joins
  };

  private static Product MapToEntity(ProductDto dto) => new()
  {
    ProductID = dto.ProductID,
    Name = dto.Name,
    ProductNumber = dto.ProductNumber,
    Color = dto.Color,
    ListPrice = dto.ListPrice,
    MakeFlag = true,
    FinishedGoodsFlag = true,
    SafetyStockLevel = 1000,
    ReorderPoint = 750,
    StandardCost = dto.ListPrice * 0.6m, // Default cost as 60% of list price
    DaysToManufacture = 0,
    SellStartDate = DateTime.UtcNow, // Required field
    rowguid = Guid.NewGuid(),
    ModifiedDate = DateTime.UtcNow
  };
}