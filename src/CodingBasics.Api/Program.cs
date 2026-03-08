using CodingBasics.Application;
using CodingBasics.Infrastructure;
using CodingBasics.Domain.Contracts;
using CodingBasics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // development convenience: some AdventureWorks tables include insert triggers
    // which block EF Core's OUTPUT clause.  Automatically disable them so the
    // API works out of the box and you don't have to run SQL manually.
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AdventureWorksDbContext>();
    db.Database.ExecuteSqlRaw("ALTER TABLE Person.Person DISABLE TRIGGER ALL;");
    db.Database.ExecuteSqlRaw("ALTER TABLE Production.Product DISABLE TRIGGER ALL;");
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
    var paged = await service.GetAllAsync(page, pageSize, ct);
    var response = new
    {
        page,
        pageSize,
        totalCount = paged.TotalCount,
        items = paged.Items
    };
    return Results.Ok(response);
})
.WithName("GetAllPeople")
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
.WithTags("People")
.WithOpenApi();

app.MapPost("/api/people", async (
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    try
    {
        var result = await service.CreateAsync(dto, ct);
        return Results.Created($"/api/people/{result.BusinessEntityID}", result);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreatePerson")
.WithTags("People")
.WithOpenApi();

app.MapPut("/api/people/{id:int}", async (
    int id,
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    try
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return Results.Ok(result);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("UpdatePerson")
.WithTags("People")
.WithOpenApi();

app.MapDelete("/api/people/{id:int}", async (
    int id,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeletePerson")
.WithTags("People")
.WithOpenApi();

// ============================================
// PRODUCTS ENDPOINTS
// ============================================

app.MapGet("/api/products", async (
    [FromServices] IProductService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;
    var paged = await service.GetAllAsync(page, pageSize, ct);
    var response = new
    {
        page,
        pageSize,
        totalCount = paged.TotalCount,
        items = paged.Items
    };
    return Results.Ok(response);
})
.WithName("GetAllProducts")
.WithTags("Products")
.WithOpenApi();

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
.WithTags("Products")
.WithOpenApi();

app.MapPost("/api/products", async (
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/products/{result.ProductID}", result);
})
.WithName("CreateProduct")
.WithTags("Products")
.WithOpenApi();

app.MapPut("/api/products/{id:int}", async (
    int id,
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.UpdateAsync(id, dto, ct);
    return Results.Ok(result);
})
.WithName("UpdateProduct")
.WithTags("Products")
.WithOpenApi();

app.MapDelete("/api/products/{id:int}", async (
    int id,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeleteProduct")
.WithTags("Products")
.WithOpenApi();

app.Run();