using Microsoft.Extensions.DependencyInjection;
using CodingBasics.Domain.Contracts;
using CodingBasics.Application.Services;

namespace CodingBasics.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Application layer services.
    /// TODO (Workshop): Uncomment service registrations after implementing services.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}