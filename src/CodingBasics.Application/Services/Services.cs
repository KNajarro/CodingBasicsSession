using CodingBasics.Domain.Contracts;

namespace CodingBasics.Application.Services;

/// <summary>
/// Person service implementation.
/// TODO (Workshop): Inject IPersonRepository via constructor and implement full CRUD methods.
/// </summary>
public sealed class PersonService : IPersonService
{
    // TODO: private readonly IPersonRepository _repository;

    // TODO: public PersonService(IPersonRepository repository)
    // {
    //     _repository = repository;
    // }

    public Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken ct = default)
    {
        // TODO: return _repository.GetAllAsync(ct);
        throw new NotImplementedException("Workshop: Implement GetAllAsync");
    }

    public Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        // TODO: return _repository.SearchAsync(name, personType, ct);
        throw new NotImplementedException("Workshop: Implement SearchAsync");
    }

    // TODO: Implement CreateAsync, UpdateAsync, DeleteAsync for Person
}

/// <summary>
/// Product service implementation.
/// TODO (Workshop): Inject IProductRepository via constructor and implement methods.
/// </summary>
public sealed class ProductService : IProductService
{
    // TODO: private readonly IProductRepository _repository;

    // TODO: public ProductService(IProductRepository repository)
    // {
    //     _repository = repository;
    // }

    public Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        // TODO: return _repository.GetAllAsync(ct);
        throw new NotImplementedException("Workshop: Implement GetAllAsync");
    }

    public Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        // TODO: return _repository.SearchAsync(name, categoryName, ct);
        throw new NotImplementedException("Workshop: Implement SearchAsync");
    }
}