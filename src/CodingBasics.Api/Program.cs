using CodingBasics.Application;
using CodingBasics.Infrastructure;
using CodingBasics.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AdventureWorks Workshop API", Version = "v1" });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ============================================
// PEOPLE ENDPOINTS
// ============================================

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

app.MapGet("/api/people/types", async (
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var result = await service.GetPersonTypesAsync(ct);
    return Results.Ok(result);
})
.WithName("GetPersonTypes")
.WithTags("People");

app.MapGet("/api/people/search", async (
    [FromQuery] string? name,
    [FromQuery] string? personType,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var result = await service.SearchAsync(name, personType, ct);
    return Results.Ok(result);
})
.WithName("SearchPeople")
.WithTags("People");

app.MapGet("/api/people/{id:int}", async (
    int id,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var result = await service.GetByIdAsync(id, ct);
    return result is null ? Results.NotFound() : Results.Ok(result);
})
.WithName("GetPersonById")
.WithTags("People");

app.MapPost("/api/people", async (
    PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    try
    {
        var created = await service.CreateAsync(dto, ct);
        return Results.Created($"/api/people/{created.BusinessEntityID}", created);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreatePerson")
.WithTags("People");

app.MapPut("/api/people/{id:int}", async (
    int id,
    PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    try
    {
        var updated = await service.UpdateAsync(id, dto, ct);
        return Results.Ok(updated);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("UpdatePerson")
.WithTags("People");

app.MapDelete("/api/people/{id:int}", async (
    int id,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
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

app.MapGet("/api/products/categories", async (
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.GetProductCategoriesAsync(ct);
    return Results.Ok(result);
})
.WithName("GetProductCategories")
.WithTags("Products");

app.MapGet("/api/products/search", async (
    [FromQuery] string? name,
    [FromQuery] string? categoryName,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.SearchAsync(name, categoryName, ct);
    return Results.Ok(result);
})
.WithName("SearchProducts")
.WithTags("Products");

app.MapGet("/api/products/{id:int}", async (
    int id,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.GetByIdAsync(id, ct);
    return result is null ? Results.NotFound() : Results.Ok(result);
})
.WithName("GetProductById")
.WithTags("Products");

app.MapPost("/api/products", async (
    ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    try
    {
        var created = await service.CreateAsync(dto, ct);
        return Results.Created($"/api/products/{created.ProductID}", created);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateProduct")
.WithTags("Products");

app.MapPut("/api/products/{id:int}", async (
    int id,
    ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    try
    {
        var updated = await service.UpdateAsync(id, dto, ct);
        return Results.Ok(updated);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("UpdateProduct")
.WithTags("Products");

app.MapDelete("/api/products/{id:int}", async (
    int id,
    [FromServices] IProductService service,
    CancellationToken ct) =>
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