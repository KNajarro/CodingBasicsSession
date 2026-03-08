namespace CodingBasics.Domain.Contracts;

/// <summary>
/// DTO representing a Person record for API responses.
/// Mapped from Person.Person table.
/// TODO (Workshop): Ensure DTO supports full CRUD operations (Create, Read, Update, Delete)
/// </summary>
public sealed class PersonDto
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

    /// <summary>
    /// Computed full name for display purposes.
    /// </summary>
    public string FullName => string.Join(" ", new[] { Title, FirstName, MiddleName, LastName, Suffix }
        .Where(s => !string.IsNullOrWhiteSpace(s)));
}

/// <summary>
/// DTO representing a Product record with category information for API responses.
/// Requires joins: Product -> ProductSubcategory -> ProductCategory
/// TODO (Workshop): Ensure DTO supports full CRUD operations (Create, Read, Update, Delete)
/// </summary>
public sealed class ProductDto
{
    // Note: For simplicity, I include all Product fields here for the Create and Update operations.
    public int ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public string? Color { get; set; }
    public decimal ListPrice { get; set; }
    public bool MakeFlag { get; set; }
    public bool FinishedGoodsFlag { get; set; }
    public short SafetyStockLevel { get; set; }
    public short ReorderPoint { get; set; }
    public decimal StandardCost { get; set; }
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

    /// <summary>
    /// Category name from Production.ProductCategory (via ProductSubcategory join).
    /// Examples: "Bikes", "Components", "Clothing", "Accessories"
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Subcategory name from Production.ProductSubcategory.
    /// Examples: "Mountain Bikes", "Road Bikes", "Handlebars", "Jerseys"
    /// </summary>
    public string? SubcategoryName { get; set; }
}