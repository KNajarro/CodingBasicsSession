using CodingBasics.Domain.Contracts;

namespace CodingBasics.Application.Services;

public sealed class PersonService : IPersonService
{
    private static readonly HashSet<string> ValidPersonTypes = ["EM", "SP", "SC", "IN", "VC", "GC"];

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

    public async Task<PersonDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _repository.GetByIdAsync(id, ct);
    }

    public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        ValidatePersonDto(dto);
        return await _repository.CreateAsync(dto, ct);
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        ValidatePersonDto(dto);
        return await _repository.UpdateAsync(id, dto, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
    }

    private static void ValidatePersonDto(PersonDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            throw new ArgumentException("FirstName is required.", nameof(dto));
        if (string.IsNullOrWhiteSpace(dto.LastName))
            throw new ArgumentException("LastName is required.", nameof(dto));
        if (string.IsNullOrWhiteSpace(dto.PersonType))
            throw new ArgumentException("PersonType is required.", nameof(dto));
        if (!ValidPersonTypes.Contains(dto.PersonType))
            throw new ArgumentException($"PersonType must be one of: {string.Join(", ", ValidPersonTypes)}.", nameof(dto));
    }
}

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

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _repository.GetByIdAsync(id, ct);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        ValidateProductDto(dto);
        return await _repository.CreateAsync(dto, ct);
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        ValidateProductDto(dto);
        return await _repository.UpdateAsync(id, dto, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
    }

    private static void ValidateProductDto(ProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name is required.", nameof(dto));
        if (string.IsNullOrWhiteSpace(dto.ProductNumber))
            throw new ArgumentException("ProductNumber is required.", nameof(dto));
        if (dto.ListPrice < 0)
            throw new ArgumentException("ListPrice must be non-negative.", nameof(dto));
    }
}