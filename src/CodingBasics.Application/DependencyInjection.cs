using Microsoft.Extensions.DependencyInjection;
using CodingBasics.Domain.Contracts;
using CodingBasics.Application.Services;

namespace CodingBasics.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IProductService, ProductService>();

        //Agregado para el funcionamiento del Dashboard//
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}