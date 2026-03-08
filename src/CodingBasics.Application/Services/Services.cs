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

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        return await _repository.SearchAsync(name, personType, ct);
    }
      
    // TODO: Implement CreateAsync, UpdateAsync, DeleteAsync for Person

    public async Task<PersonDto> CreateAsync(PersonDto person, CancellationToken ct = default)
    {
        // Llama al repositorio para insertar una nueva persona
        return await _repository.CreateAsync(person, ct);
    }

    public async Task<PersonDto> UpdateAsync(PersonDto person, CancellationToken ct = default)
    {
        // Envía los cambios de una persona existente al repositorio
        return await _repository.UpdateAsync(person, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        // Ordena al repositorio eliminar por el ID (BusinessEntityID)
        await _repository.DeleteAsync(id, ct);
    }
}

/// <summary>
/// Product service implementation.
/// TODO (Workshop): Inject IProductRepository via constructor and implement methods.
/// </summary>
public sealed class ProductService : IProductService
{
    // 1. Declaramos la herramienta (Repositorio)
    private readonly IProductRepository _repository;

    // 2. El Constructor: Aquí es donde "Inyectamos" el repositorio
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    // 3. Implementamos el método para traer todo
    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }

    // 4. Implementamos el método de búsqueda y filtros
    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        return await _repository.SearchAsync(name, categoryName, ct);
    }
}