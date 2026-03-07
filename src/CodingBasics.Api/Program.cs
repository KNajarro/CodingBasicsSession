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

// app.MapGet("/api/people/search", async (
//     string? name,
//     string? personType,
//     IPersonService service,
//     CancellationToken ct) =>
// {
//     // TODO: Implement search people
//     throw new NotImplementedException("Workshop: Implement GET /api/people/search");
// })
// .WithName("SearchPeople")
// .WithTags("People")
// .WithOpenApi();

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

// app.MapGet("/api/products", async (IProductService service, CancellationToken ct) =>
// {
//     // var result = await service.GetAllAsync(ct);
//     // return Results.Ok(result);
//     throw new NotImplementedException("Workshop: Implement GET /api/products");
// })
// .WithName("GetAllProducts")
// .WithTags("Products")
// .WithOpenApi();

// app.MapGet("/api/products/search", async (
//     string? name,
//     string? categoryName,
//     IProductService service,
//     CancellationToken ct) =>
// {
//     // TODO (Workshop): Implement endpoint with optional filters
//     // var result = await service.SearchAsync(name, categoryName, ct);
//     // return Results.Ok(result);
//     throw new NotImplementedException("Workshop: Implement GET /api/products/search");
// })
// .WithName("SearchProducts")
// .WithTags("Products")
// .WithOpenApi();

app.Run();