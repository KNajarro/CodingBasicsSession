using CodingBasics.Domain.Contracts;

namespace CodingBasics.Application.Services;

/// <summary>
/// Person service implementation.
/// TODO (Workshop): Inject IPersonRepository via constructor and implement full CRUD methods.
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

    public Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        return _repository.SearchAsync(name, personType, ct);
    }

    // TODO: Implement CreateAsync, UpdateAsync, DeleteAsync for Person
}

/// <summary>
/// Product service implementation.
/// TODO (Workshop): Inject IProductRepository via constructor and implement methods.
/// </summary>
public sealed class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        return _repository.GetAllAsync(ct);
    }

    public Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        return _repository.SearchAsync(name, categoryName, ct);
    }
}