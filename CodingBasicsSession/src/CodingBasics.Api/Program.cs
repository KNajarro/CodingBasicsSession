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

// Add CORS policy for frontend development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
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

// Enable CORS
app.UseCors("AllowAllOrigins");

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
.WithTags("People");

app.MapGet("/api/people/search", async (
    [FromQuery] string? name,
    [FromQuery] string? personType,
    [FromServices] IPersonService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 50) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 50;
    if (pageSize > 200) pageSize = 200;

    var result = await service.SearchAsync(name, personType, ct);
    var totalCount = result.Count();
    var items = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    return Results.Ok(new
    {
        page,
        pageSize,
        totalCount,
        items
    });
})
.WithName("SearchPeople")
.WithTags("People");

app.MapPost("/api/people", async (
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/people/{result.BusinessEntityID}", result);
})
.WithName("CreatePerson")
.WithTags("People");

app.MapPut("/api/people/{id}", async (
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
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"Person with ID {id} not found");
    }
})
.WithName("UpdatePerson")
.WithTags("People");

app.MapDelete("/api/people/{id}", async (
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
        return Results.NotFound($"Person with ID {id} not found");
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
    [FromQuery] int pageSize = 20) =>
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
.WithTags("Products");

app.MapGet("/api/products/search", async (
    [FromQuery] string? name,
    [FromQuery] string? categoryName,
    [FromServices] IProductService service,
    CancellationToken ct,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 50) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 50;
    if (pageSize > 200) pageSize = 200;

    var result = await service.SearchAsync(name, categoryName, ct);
    var totalCount = result.Count();
    var items = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    return Results.Ok(new
    {
        page,
        pageSize,
        totalCount,
        items
    });
})
.WithName("SearchProducts")
.WithTags("Products");

app.MapPost("/api/products", async (
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(dto, ct);
    return Results.Created($"/api/products/{result.ProductID}", result);
})
.WithName("CreateProduct")
.WithTags("Products");

app.MapPut("/api/products/{id}", async (
    int id,
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    try
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return Results.Ok(result);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"Product with ID {id} not found");
    }
})
.WithName("UpdateProduct")
.WithTags("Products");

app.MapDelete("/api/products/{id}", async (
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
        return Results.NotFound($"Product with ID {id} not found");
    }
})
.WithName("DeleteProduct")
.WithTags("Products");

app.Run();