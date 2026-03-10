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

    public async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        return await _repository.SearchAsync(name, personType, ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        return await _repository.CreateAsync(dto, ct);
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        return await _repository.UpdateAsync(id, dto, ct);
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

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        return await _repository.SearchAsync(name, categoryName, ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        return await _repository.CreateAsync(dto, ct);
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        return await _repository.UpdateAsync(id, dto, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
    }
}