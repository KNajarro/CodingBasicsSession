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

app.MapGet("/api/people", async (IPersonService service, CancellationToken ct) =>
{
    // TODO (Workshop): Implement endpoint
    // var result = await service.GetAllAsync(ct);
    // return Results.Ok(result);
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
    // TODO (Workshop): Implement endpoint with optional filters
    // var result = await service.SearchAsync(name, personType, ct);
    // return Results.Ok(result);
    throw new NotImplementedException("Workshop: Implement GET /api/people/search");
})
.WithName("SearchPeople")
.WithTags("People")
.WithOpenApi();

// ============================================
// PRODUCTS ENDPOINTS
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