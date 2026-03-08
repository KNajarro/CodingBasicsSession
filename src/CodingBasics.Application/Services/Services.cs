using System.Formats.Asn1;
using CodingBasics.Domain.Contracts;
using CodingBasics.Domain.AdventureWorks.Entities;
using System.Reflection.Metadata.Ecma335;

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
        var people = await _repository.GetAllAsync(ct);
        return people.Select(p => new PersonDto
    {
        BusinessEntityID = p.BusinessEntityID,
        PersonType = p.PersonType,
        NameStyle = p.NameStyle,
        Title = p.Title,
        FirstName = p.FirstName,
        MiddleName = p.MiddleName,
        LastName = p.LastName,
        Suffix = p.Suffix,
        EmailPromotion = p.EmailPromotion,
        AdditionalContactInfo = p.AdditionalContactInfo,
        Demographics = p.Demographics
    });
    }

    public async Task<IEnumerable<PersonDto>> SearchAsync(string? name, string? personType, CancellationToken ct = default)
    {
        var people = await _repository.SearchAsync(name, personType, ct);

        return people.Select(p => new PersonDto
        {
            BusinessEntityID = p.BusinessEntityID,
            PersonType = p.PersonType,
            NameStyle = p.NameStyle,
            Title = p.Title,
            FirstName = p.FirstName,
            MiddleName = p.MiddleName,
            LastName = p.LastName,
            Suffix = p.Suffix,
            EmailPromotion = p.EmailPromotion,
            AdditionalContactInfo = p.AdditionalContactInfo,
            Demographics = p.Demographics
        });
    }

   public async Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default)
    {
        var person = new Person
        {
            BusinessEntityID = dto.BusinessEntityID,
            PersonType = dto.PersonType,
            NameStyle = dto.NameStyle,
            Title = dto.Title,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Suffix = dto.Suffix,
            EmailPromotion = dto.EmailPromotion,
            AdditionalContactInfo = dto.AdditionalContactInfo,
            Demographics = dto.Demographics
        };

        var createdPerson = await _repository.CreateAsync(person, ct);

        return new PersonDto
        {
            BusinessEntityID = createdPerson.BusinessEntityID,
            PersonType = createdPerson.PersonType,
            NameStyle = createdPerson.NameStyle,
            Title = createdPerson.Title,
            FirstName = createdPerson.FirstName,
            MiddleName = createdPerson.MiddleName,
            LastName = createdPerson.LastName,
            Suffix = createdPerson.Suffix,
            EmailPromotion = createdPerson.EmailPromotion,
            AdditionalContactInfo = createdPerson.AdditionalContactInfo,
            Demographics = createdPerson.Demographics
        };
    }

    public async Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default)
    {
        var person = new Person
        {
            BusinessEntityID = dto.BusinessEntityID,
            PersonType = dto.PersonType,
            NameStyle = dto.NameStyle,
            Title = dto.Title,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Suffix = dto.Suffix,
            EmailPromotion = dto.EmailPromotion,
            AdditionalContactInfo = dto.AdditionalContactInfo,
            Demographics = dto.Demographics
        };

        var updatedPerson = await _repository.UpdateAsync(id, person, ct);

        return new PersonDto
        {
            BusinessEntityID = updatedPerson.BusinessEntityID,
            PersonType = updatedPerson.PersonType,
            NameStyle = updatedPerson.NameStyle,
            Title = updatedPerson.Title,
            FirstName = updatedPerson.FirstName,
            MiddleName = updatedPerson.MiddleName,
            LastName = updatedPerson.LastName,
            Suffix = updatedPerson.Suffix,
            EmailPromotion = updatedPerson.EmailPromotion,
            AdditionalContactInfo = updatedPerson.AdditionalContactInfo,
            Demographics = updatedPerson.Demographics
        };
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
    {
        
        return _repository.DeleteAsync(id, ct);
    }

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