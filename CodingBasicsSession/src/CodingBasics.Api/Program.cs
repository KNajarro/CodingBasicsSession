using CodingBasics.Application;
using CodingBasics.Infrastructure;
using CodingBasics.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AdventureWorks Workshop API", Version = "v1" });
});

// Register application and infrastructure layers
builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ============================================
// PEOPLE ENDPOINTS
// ============================================

// GET /api/people - Get all people (paginated)
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
    return Results.Ok(new { page, pageSize, totalCount, items });
})
.WithName("GetAllPeople")
.WithTags("People");

// GET /api/people/search - Search people
app.MapGet("/api/people/search", async (
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] string? name = null,
    [FromQuery] string? personType = null
) =>
{
    var result = await service.SearchAsync(name, personType, ct);
    return Results.Ok(result);
})
.WithName("SearchPeople")
.WithTags("People");

// GET /api/people/by-name/{name} - Get people by name
app.MapGet("/api/people/by-name/{name}", async (
    string name,
    [FromServices] IPersonService service,
    CancellationToken ct
) =>
{
    var result = await service.SearchAsync(name, null, ct);
    return Results.Ok(result);
})
.WithName("GetPeopleByName")
.WithTags("People");

// GET /api/people/by-type/{personType} - Get people by person type
app.MapGet("/api/people/by-type/{personType}", async (
    string personType,
    [FromServices] IPersonService service,
    CancellationToken ct
) =>
{
    var result = await service.SearchAsync(null, personType, ct);
    return Results.Ok(result);
})
.WithName("GetPeopleByType")
.WithTags("People");

// GET /api/people/by-name-and-type/{name}/{personType} - Get people by name and person type
app.MapGet("/api/people/by-name-and-type/{name}/{personType}", async (
    string name,
    string personType,
    [FromServices] IPersonService service,
    CancellationToken ct
) =>
{
    var result = await service.SearchAsync(name, personType, ct);
    return Results.Ok(result);
})
.WithName("GetPeopleByNameAndType")
.WithTags("People");

// POST /api/people - Create a new person
app.MapPost("/api/people", async (
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct
) =>
{
    var created = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/people/{created.BusinessEntityID}", created);
})
.WithName("CreatePerson")
.WithTags("People");

// PUT /api/people/{id} - Update a person
app.MapPut("/api/people/{id}", async (
    int id,
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct
) =>
{
    try
    {
        var updated = await service.UpdateAsync(id, dto, ct);
        return Results.Ok(updated);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("UpdatePerson")
.WithTags("People");

// DELETE /api/people/{id} - Delete a person
app.MapDelete("/api/people/{id}", async (
    int id,
    [FromServices] IPersonService service,
    CancellationToken ct
) =>
{
    try
    {
        await service.DeleteAsync(id, ct);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("DeletePerson")
.WithTags("People");

// ============================================
// PRODUCTS ENDPOINTS
// ============================================

// GET /api/products - Get all products (paginated)
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
    return Results.Ok(new { page, pageSize, totalCount, items });
})
.WithName("GetAllProducts")
.WithTags("Products");

// GET /api/products/search - Search products
app.MapGet("/api/products/search", async (
    [FromServices] IProductService service,
    CancellationToken ct,
    [FromQuery] string? name = null,
    [FromQuery] string? categoryName = null
) =>
{
    var result = await service.SearchAsync(name, categoryName, ct);
    return Results.Ok(result);
})
.WithName("SearchProducts")
.WithTags("Products");

// GET /api/products/by-name/{name} - Get products by name
app.MapGet("/api/products/by-name/{name}", async (
    string name,
    [FromServices] IProductService service,
    CancellationToken ct
) =>
{
    var result = await service.SearchAsync(name, null, ct);
    return Results.Ok(result);
})
.WithName("GetProductsByName")
.WithTags("Products");

// GET /api/products/by-category/{categoryName} - Get products by category
app.MapGet("/api/products/by-category/{categoryName}", async (
    string categoryName,
    [FromServices] IProductService service,
    CancellationToken ct
) =>
{
    var result = await service.SearchAsync(null, categoryName, ct);
    return Results.Ok(result);
})
.WithName("GetProductsByCategory")
.WithTags("Products");

// POST /api/products - Create a new product
app.MapPost("/api/products", async (
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct
) =>
{
    var created = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/products/{created.ProductID}", created);
})
.WithName("CreateProduct")
.WithTags("Products");

// PUT /api/products/{id} - Update a product
app.MapPut("/api/products/{id}", async (
    int id,
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct
) =>
{
    try
    {
        var updated = await service.UpdateAsync(id, dto, ct);
        return Results.Ok(updated);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("UpdateProduct")
.WithTags("Products");

// DELETE /api/products/{id} - Delete a product
app.MapDelete("/api/products/{id}", async (
    int id,
    [FromServices] IProductService service,
    CancellationToken ct
) =>
{
    try
    {
        await service.DeleteAsync(id, ct);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("DeleteProduct")
.WithTags("Products");

app.Run();