using CodingBasics.Domain.Contracts;
using CodingBasics.Domain.AdventureWorks.Entities;


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
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        var products = await _repository.GetAllAsync(ct);

        return products.Select(p => new ProductDto
        {
            ProductID = p.ProductID,
            Name = p.Name,
            ProductNumber = p.ProductNumber,
            Color = p.Color,
            ListPrice = p.ListPrice,
            MakeFlag = p.MakeFlag,
            FinishedGoodsFlag = p.FinishedGoodsFlag,
            SafetyStockLevel = p.SafetyStockLevel,
            ReorderPoint = p.ReorderPoint,
            StandardCost = p.StandardCost,
            Size = p.Size,
            SizeUnitMeasureCode = p.SizeUnitMeasureCode,
            WeightUnitMeasureCode = p.WeightUnitMeasureCode,
            Weight = p.Weight,
            DaysToManufacture = p.DaysToManufacture,
            ProductLine = p.ProductLine,
            Class = p.Class,
            Style = p.Style,
            ProductSubcategoryID = p.ProductSubcategoryID,
            ProductModelID = p.ProductModelID,
            SellStartDate = p.SellStartDate,
            SellEndDate = p.SellEndDate,
            DiscontinuedDate = p.DiscontinuedDate
        });
    }

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? categoryName, CancellationToken ct = default)
    {
        var products = await _repository.SearchAsync(name, categoryName, ct);

        return products.Select(p => new ProductDto
        {
            ProductID = p.ProductID,
            Name = p.Name,
            ProductNumber = p.ProductNumber,
            Color = p.Color,
            ListPrice = p.ListPrice,
            MakeFlag = p.MakeFlag,
            FinishedGoodsFlag = p.FinishedGoodsFlag,
            SafetyStockLevel = p.SafetyStockLevel,
            ReorderPoint = p.ReorderPoint,
            StandardCost = p.StandardCost,
            Size = p.Size,
            SizeUnitMeasureCode = p.SizeUnitMeasureCode,
            WeightUnitMeasureCode = p.WeightUnitMeasureCode,
            Weight = p.Weight,
            DaysToManufacture = p.DaysToManufacture,
            ProductLine = p.ProductLine,
            Class = p.Class,
            Style = p.Style,
            ProductSubcategoryID = p.ProductSubcategoryID,
            ProductModelID = p.ProductModelID,
            SellStartDate = p.SellStartDate,
            SellEndDate = p.SellEndDate,
            DiscontinuedDate = p.DiscontinuedDate
        });
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var product = new Product
        {
            ProductID = dto.ProductID,
            Name = dto.Name,
            ProductNumber = dto.ProductNumber,
            Color = dto.Color,
            ListPrice = dto.ListPrice,
            MakeFlag = dto.MakeFlag,
            FinishedGoodsFlag = dto.FinishedGoodsFlag,
            SafetyStockLevel = dto.SafetyStockLevel,
            ReorderPoint = dto.ReorderPoint,
            StandardCost = dto.StandardCost,
            Size = dto.Size,
            SizeUnitMeasureCode = dto.SizeUnitMeasureCode,
            WeightUnitMeasureCode = dto.WeightUnitMeasureCode,
            Weight = dto.Weight,
            DaysToManufacture = dto.DaysToManufacture,
            ProductLine = dto.ProductLine,
            Class = dto.Class,
            Style = dto.Style,
            ProductSubcategoryID = dto.ProductSubcategoryID,
            ProductModelID = dto.ProductModelID,
            SellStartDate = dto.SellStartDate,
            SellEndDate = dto.SellEndDate,
            DiscontinuedDate = dto.DiscontinuedDate
        };

        var created = await _repository.CreateAsync(product, ct);

        return new ProductDto
        {
            ProductID = created.ProductID,
            Name = created.Name,
            ProductNumber = created.ProductNumber,
            Color = created.Color,
            ListPrice = created.ListPrice,
            MakeFlag = created.MakeFlag,
            FinishedGoodsFlag = created.FinishedGoodsFlag,
            SafetyStockLevel = created.SafetyStockLevel,
            ReorderPoint = created.ReorderPoint,
            StandardCost = created.StandardCost,
            Size = created.Size,
            SizeUnitMeasureCode = created.SizeUnitMeasureCode,
            WeightUnitMeasureCode = created.WeightUnitMeasureCode,
            Weight = created.Weight,
            DaysToManufacture = created.DaysToManufacture,
            ProductLine = created.ProductLine,
            Class = created.Class,
            Style = created.Style,
            ProductSubcategoryID = created.ProductSubcategoryID,
            ProductModelID = created.ProductModelID,
            SellStartDate = created.SellStartDate,
            SellEndDate = created.SellEndDate,
            DiscontinuedDate = created.DiscontinuedDate
        };
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default)
    {
        var product = new Product
        {
            ProductID = dto.ProductID,
            Name = dto.Name,
            ProductNumber = dto.ProductNumber,
            Color = dto.Color,
            ListPrice = dto.ListPrice,
            MakeFlag = dto.MakeFlag,
            FinishedGoodsFlag = dto.FinishedGoodsFlag,
            SafetyStockLevel = dto.SafetyStockLevel,
            ReorderPoint = dto.ReorderPoint,
            StandardCost = dto.StandardCost,
            Size = dto.Size,
            SizeUnitMeasureCode = dto.SizeUnitMeasureCode,
            WeightUnitMeasureCode = dto.WeightUnitMeasureCode,
            Weight = dto.Weight,
            DaysToManufacture = dto.DaysToManufacture,
            ProductLine = dto.ProductLine,
            Class = dto.Class,
            Style = dto.Style,
            ProductSubcategoryID = dto.ProductSubcategoryID,
            ProductModelID = dto.ProductModelID,
            SellStartDate = dto.SellStartDate,
            SellEndDate = dto.SellEndDate,
            DiscontinuedDate = dto.DiscontinuedDate
        };

        var updated = await _repository.UpdateAsync(id, product, ct);

        return new ProductDto
        {
            ProductID = updated.ProductID,
            Name = updated.Name,
            ProductNumber = updated.ProductNumber,
            Color = updated.Color,
            ListPrice = updated.ListPrice,
            MakeFlag = updated.MakeFlag,
            FinishedGoodsFlag = updated.FinishedGoodsFlag,
            SafetyStockLevel = updated.SafetyStockLevel,
            ReorderPoint = updated.ReorderPoint,
            StandardCost = updated.StandardCost,
            Size = updated.Size,
            SizeUnitMeasureCode = updated.SizeUnitMeasureCode,
            WeightUnitMeasureCode = updated.WeightUnitMeasureCode,
            Weight = updated.Weight,
            DaysToManufacture = updated.DaysToManufacture,
            ProductLine = updated.ProductLine,
            Class = updated.Class,
            Style = updated.Style,
            ProductSubcategoryID = updated.ProductSubcategoryID,
            ProductModelID = updated.ProductModelID,
            SellStartDate = updated.SellStartDate,
            SellEndDate = updated.SellEndDate,
            DiscontinuedDate = updated.DiscontinuedDate
        };
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
    {
        return _repository.DeleteAsync(id, ct);
    }
}