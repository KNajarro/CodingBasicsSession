namespace CodingBasics.Domain.AdventureWorks.Entities;

/// <summary>
/// Maps to Person.Person table in AdventureWorks database.
/// Schema: Person | Primary Key: BusinessEntityID
/// </summary>
public sealed class Person
{
    public int BusinessEntityID { get; set; }
    public string PersonType { get; set; } = string.Empty;
    public bool NameStyle { get; set; }
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int EmailPromotion { get; set; }
    public string? AdditionalContactInfo { get; set; }
    public string? Demographics { get; set; }
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}

/// <summary>
/// Maps to Production.Product table in AdventureWorks database.
/// Schema: Production | Primary Key: ProductID
/// </summary>
public sealed class Product
{
    public int ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public bool MakeFlag { get; set; }
    public bool FinishedGoodsFlag { get; set; }
    public string? Color { get; set; }
    public short SafetyStockLevel { get; set; }
    public short ReorderPoint { get; set; }
    public decimal StandardCost { get; set; }
    public decimal ListPrice { get; set; }
    public string? Size { get; set; }
    public string? SizeUnitMeasureCode { get; set; }
    public string? WeightUnitMeasureCode { get; set; }
    public decimal? Weight { get; set; }
    public int DaysToManufacture { get; set; }
    public string? ProductLine { get; set; }
    public string? Class { get; set; }
    public string? Style { get; set; }
    public int? ProductSubcategoryID { get; set; }
    public int? ProductModelID { get; set; }
    public DateTime? SellStartDate { get; set; }
    public DateTime? SellEndDate { get; set; }
    public DateTime? DiscontinuedDate { get; set; }
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}

/// <summary>
/// Maps to Production.ProductSubcategory table in AdventureWorks database.
/// Schema: Production | Primary Key: ProductSubcategoryID
/// Foreign Key: ProductCategoryID -> Production.ProductCategory
/// </summary>
public sealed class ProductSubcategory
{
    public int ProductSubcategoryID { get; set; }
    public int ProductCategoryID { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}

/// <summary>
/// Maps to Production.ProductCategory table in AdventureWorks database.
/// Schema: Production | Primary Key: ProductCategoryID
/// </summary>
public sealed class ProductCategory
{
    public int ProductCategoryID { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}