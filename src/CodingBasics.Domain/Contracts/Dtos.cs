namespace CodingBasics.Domain.Contracts;

/// <summary>
/// DTO representing a Person record for API responses.
/// Mapped from Person.Person table.
/// </summary>
public sealed class PersonDto
{
    public int BusinessEntityID { get; set; }
    public string PersonType { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int EmailPromotion { get; set; }

    /// <summary>
    /// Computed full name for display purposes.
    /// </summary>
    public string FullName => string.Join(" ", new[] { Title, FirstName, MiddleName, LastName, Suffix }
        .Where(s => !string.IsNullOrWhiteSpace(s)));
}

/// <summary>
/// DTO representing a Product record with category information for API responses.
/// Requires joins: Product -> ProductSubcategory -> ProductCategory
/// </summary>
public sealed class ProductDto
{
    public int ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public string? Color { get; set; }
    public decimal ListPrice { get; set; }

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