using CodingBasics.Application;
using CodingBasics.Infrastructure;
using CodingBasics.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "AdventureWorks Workshop API", 
        Version = "v1",
        Description = "A comprehensive API for managing AdventureWorks data including People and Products"
    });

    // Enable XML documentation from comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Register application and infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AdventureWorks Workshop API v1");
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        options.EnableTryItOutByDefault();
    });
}

// ============================================
// PEOPLE ENDPOINTS
// ============================================

// GET /api/people - Retrieves all people with pagination support
// Example: GET /api/people?page=1&pageSize=20
app.MapGet("/api/people", async (
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20
) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.GetAllAsync(ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetAllPeople")
.WithOpenApi()
.WithTags("People")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get all people (paginated)")
.WithDescription("Retrieves a paginated list of all people in the system. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// GET /api/people/search - Searches for people using optional filters with pagination
// Example: GET /api/people/search?name=John&personType=EM&page=1&pageSize=20
app.MapGet("/api/people/search", async (
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] string? name = null,
    [FromQuery] string? personType = null,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.SearchAsync(name, personType, ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("SearchPeople")
.WithOpenApi()
.WithTags("People")
.Produces(200, typeof(object), "application/json")
.WithSummary("Search people")
.WithDescription("Searches for people by optional name and/or person type with pagination. Returns paginated results with page number, page size, total count, and items. Default page size is 20.");

// GET /api/people/by-name/{name} - Retrieves people by their name with pagination
// Example: GET /api/people/by-name/John?page=1&pageSize=20
app.MapGet("/api/people/by-name/{name}", async (
    [FromRoute] string name,
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.SearchAsync(name, null, ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetPersonByName")
.WithOpenApi()
.WithTags("People")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get people by name")
.WithDescription("Retrieves people by searching their first or last name with pagination. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// GET /api/people/by-type/{personType} - Retrieves people by their person type with pagination
// Example: GET /api/people/by-type/EM?page=1&pageSize=20
app.MapGet("/api/people/by-type/{personType}", async (
    [FromRoute] string personType,
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.SearchAsync(null, personType, ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetPersonByPersonType")
.WithOpenApi()
.WithTags("People")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get people by person type")
.WithDescription("Retrieves people by their person type classification with pagination. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// GET /api/people/by-name-and-type/{name}/{personType} - Retrieves people by both name and type with pagination
// Route parameters: name and personType
// Example: GET /api/people/by-name-and-type/John/EM?page=1&pageSize=20
app.MapGet("/api/people/by-name-and-type/{name}/{personType}", async (
    [FromRoute] string name,
    [FromRoute] string personType,
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.SearchAsync(name, personType, ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetPersonByNameAndPersonType")
.WithOpenApi()
.WithTags("People")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get people by name and person type")
.WithDescription("Retrieves people by both name and person type filters with pagination. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// POST /api/people - Creates a new person
// Request body: { personType, title, firstName, middleName, lastName, suffix, emailPromotion }
// Returns: 201 Created with the new person including assigned businessEntityID
app.MapPost("/api/people", async (
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/people/{result.BusinessEntityID}", result);
})
.WithName("CreatePerson")
.WithOpenApi()
.WithTags("People")
.Accepts<PersonDto>("application/json")
.Produces(201, typeof(PersonDto), "application/json")
.WithSummary("Create a new person")
.WithDescription("Creates a new person record. The BusinessEntityID is auto-generated and returned in the response.");

// PUT /api/people/{id} - Updates an existing person
// Route parameter: id (BusinessEntityID)
// Request body: { personType, title, firstName, middleName, lastName, suffix, emailPromotion }
// Returns: 200 OK with the updated person, or 404 if not found
app.MapPut("/api/people/{id}", async (
    [FromRoute] int id,
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var result = await service.UpdateAsync(id, dto, ct);
    return Results.Ok(result);
})
.WithName("UpdatePerson")
.WithOpenApi()
.WithTags("People")
.Accepts<PersonDto>("application/json")
.Produces(200, typeof(PersonDto), "application/json")
.Produces(404)
.WithSummary("Update a person")
.WithDescription("Updates an existing person record");

// DELETE /api/people/{id} - Deletes a person
// Route parameter: id (BusinessEntityID)
// Returns: 204 No Content if successful, or 404 if not found
app.MapDelete("/api/people/{id}", async (
    [FromRoute] int id,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeletePerson")
.WithOpenApi()
.WithTags("People")
.Produces(204)
.Produces(404)
.WithSummary("Delete a person")
.WithDescription("Deletes a person record from the system");

// ============================================
// PRODUCTS ENDPOINTS
// ============================================

// GET /api/products - Retrieves all products with pagination support
// Example: GET /api/products?page=1&pageSize=20
app.MapGet("/api/products", async (
    [FromServices] IProductService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20
) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.GetAllAsync(ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetAllProducts")
.WithOpenApi()
.WithTags("Products")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get all products (paginated)")
.WithDescription("Retrieves a paginated list of all products in the system. Includes product details and category information. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// GET /api/products/by-name/{name} - Retrieves products by their name with pagination
// Example: GET /api/products/by-name/Road?page=1&pageSize=20
app.MapGet("/api/products/by-name/{name}", async (
    [FromRoute] string name,
    [FromServices] IProductService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.SearchAsync(name, null, ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetProductByName")
.WithOpenApi()
.WithTags("Products")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get products by name")
.WithDescription("Retrieves products by searching their name with pagination. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// GET /api/products/by-category/{categoryName} - Retrieves products by their category with pagination
// Example: GET /api/products/by-category/Bikes?page=1&pageSize=20
app.MapGet("/api/products/by-category/{categoryName}", async (
    [FromRoute] string categoryName,
    [FromServices] IProductService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var all = await service.SearchAsync(null, categoryName, ct);
    var totalCount = all.Count();
    var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var response = new
    {
        page,
        pageSize,
        totalCount,
        items
    };
    return Results.Ok(response);
})
.WithName("GetProductByCategory")
.WithOpenApi()
.WithTags("Products")
.Produces(200, typeof(object), "application/json")
.WithSummary("Get products by category")
.WithDescription("Retrieves products by their category name with pagination. Supports pagination with 'page' and 'pageSize' query parameters. Default page size is 20.");

// POST /api/products - Creates a new product
// Request body: { name, productNumber, color, listPrice, categoryName, subcategoryName }
// Returns: 201 Created with the new product including assigned productID
app.MapPost("/api/products", async (
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/products/{result.ProductID}", result);
})
.WithName("CreateProduct")
.WithOpenApi()
.WithTags("Products")
.Accepts<ProductDto>("application/json")
.Produces(201, typeof(ProductDto), "application/json")
.WithSummary("Create a new product")
.WithDescription("Creates a new product record. The ProductID is auto-generated and returned in the response.");

// PUT /api/products/{id} - Updates an existing product
// Route parameter: id (ProductID)
// Request body: { name, productNumber, color, listPrice, categoryName, subcategoryName }
// Returns: 200 OK with the updated product, or 404 if not found
app.MapPut("/api/products/{id}", async (
    [FromRoute] int id,
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.UpdateAsync(id, dto, ct);
    return Results.Ok(result);
})
.WithName("UpdateProduct")
.WithOpenApi()
.WithTags("Products")
.Accepts<ProductDto>("application/json")
.Produces(200, typeof(ProductDto), "application/json")
.Produces(404)
.WithSummary("Update a product")
.WithDescription("Updates an existing product record");

// DELETE /api/products/{id} - Deletes a product
// Route parameter: id (ProductID)
// Returns: 204 No Content if successful, or 404 if not found
app.MapDelete("/api/products/{id}", async (
    [FromRoute] int id,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeleteProduct")
.WithOpenApi()
.WithTags("Products")
.Produces(204)
.Produces(404)
.WithSummary("Delete a product")
.WithDescription("Deletes a product record from the system");

app.Run();
