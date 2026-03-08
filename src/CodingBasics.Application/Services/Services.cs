using CodingBasics.Domain.Contracts;

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

    private static void ValidatePerson(PersonDto dto)
    {
        if (!string.IsNullOrEmpty(dto.PersonType) && dto.PersonType.Length > 2)
        {
            throw new ArgumentException("PersonType must be at most 2 characters.", nameof(dto.PersonType));
        }
    }

    public async Task<PaginatedList<PersonDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(page, pageSize, ct);
    }

    public async Task<PaginatedList<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        var filter = $"{name} {personType}".Trim();
        return await _repository.SearchAsync(filter, 1, 1000, ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        ValidatePerson(dto);
        return await _repository.CreateAsync(dto, ct);
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        ValidatePerson(dto);
        dto.BusinessEntityID = id;
        await _repository.UpdateAsync(dto, ct);
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
    }
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

    public async Task<PaginatedList<ProductDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(page, pageSize, ct);
    }

    public async Task<PaginatedList<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        var filter = $"{name} {categoryName}".Trim();
        return await _repository.SearchAsync(filter, 1, 1000, ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        return await _repository.CreateAsync(dto, ct);
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        dto.ProductID = id;
        await _repository.UpdateAsync(dto, ct);
        return dto;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
    }
}