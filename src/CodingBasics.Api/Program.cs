using CodingBasics.Application;
using CodingBasics.Infrastructure;
using CodingBasics.Domain.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AdventureWorks Workshop API", Version = "v1" });
});

// Register application and infrastructure layers
builder.Services.AddApplication();

// TODO (Workshop): Uncomment after implementing Infrastructure layer
// builder.Services.AddInfrastructure(builder.Configuration);

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

app.MapGet("/api/people", async (IPersonService service, CancellationToken ct) =>
{
    // TODO: Implement GET all people
    throw new NotImplementedException("Workshop: Implement GET /api/people");
})
.WithName("GetAllPeople")
.WithTags("People")
.WithOpenApi();

app.MapGet("/api/people/search", async (
    string? name,
    string? personType,
    IPersonService service,
    CancellationToken ct) =>
{
    // TODO: Implement search people
    throw new NotImplementedException("Workshop: Implement GET /api/people/search");
})
.WithName("SearchPeople")
.WithTags("People")
.WithOpenApi();

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

app.MapGet("/api/products", async (IProductService service, CancellationToken ct) =>
{
    // TODO (Workshop): Implement endpoint
    // var result = await service.GetAllAsync(ct);
    // return Results.Ok(result);
    throw new NotImplementedException("Workshop: Implement GET /api/products");
})
.WithName("GetAllProducts")
.WithTags("Products")
.WithOpenApi();

app.MapGet("/api/products/search", async (
    string? name,
    string? categoryName,
    IProductService service,
    CancellationToken ct) =>
{
    // TODO (Workshop): Implement endpoint with optional filters
    // var result = await service.SearchAsync(name, categoryName, ct);
    // return Results.Ok(result);
    throw new NotImplementedException("Workshop: Implement GET /api/products/search");
})
.WithName("SearchProducts")
.WithTags("Products")
.WithOpenApi();

app.Run();