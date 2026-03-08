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

// TODO (Workshop): Implement full CRUD endpoints for People
// - GET /api/people
// - GET /api/people/search
// - POST /api/people
// - PUT /api/people/{id}
// - DELETE /api/people/{id}

/*
[FromServices]tell ASP.NET that the parameter must 
be taken from the dependency injection container, 
not from the HTTP request body, query string, or route.
*/

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


app.MapPost("/api/people", async (
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var created = await service.CreateAsync(dto, ct);
    return Results.Ok(created);
})
.WithName("CreatePerson")
.WithTags("People");

app.MapPut("/api/people/{id:int}", async (
    int id,
    [FromBody] PersonDto dto,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    var updated = await service.UpdateAsync(id, dto, ct);
    return Results.Ok(updated);
})
.WithName("UpdatePerson")
.WithTags("People");

app.MapDelete("/api/people/{id:int}", async (
    int id,
    [FromServices] IPersonService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeletePerson")
.WithTags("People");






// TODO: Implement POST /api/people (create)
// TODO: Implement PUT /api/people/{id} (update)
// TODO: Implement DELETE /api/people/{id} (delete)

// ============================================
// PRODUCTS ENDPOINTS
// TODO (Workshop): Implement full CRUD endpoints for Products
// - GET /api/products
// - GET /api/products/search
// - POST /api/products
// - PUT /api/products/{id}
// - DELETE /api/products/{id}
// ============================================

app.MapGet("/api/products", async (
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.GetAllAsync(ct);
    return Results.Ok(result);
})
.WithName("GetAllProducts")
.WithTags("Products");

app.MapGet("/api/products/search", async (
    string? name,
    string? categoryName,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var result = await service.SearchAsync(name, categoryName, ct);
    return Results.Ok(result);
})
.WithName("SearchProducts")
.WithTags("Products");

app.MapPost("/api/products", async (
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var created = await service.CreateAsync(dto, ct);
    return Results.Ok(created);
})
.WithName("CreateProduct")
.WithTags("Products");

app.MapPut("/api/products/{id:int}", async (
    int id,
    [FromBody] ProductDto dto,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    var updated = await service.UpdateAsync(id, dto, ct);
    return Results.Ok(updated);
})
.WithName("UpdateProduct")
.WithTags("Products");

app.MapDelete("/api/products/{id:int}", async (
    int id,
    [FromServices] IProductService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeleteProduct")
.WithTags("Products");

app.Run();