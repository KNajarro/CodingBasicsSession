using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CodingBasics.Domain.Contracts;
using CodingBasics.Infrastructure.Persistence;
using CodingBasics.Infrastructure.Repositories;

namespace CodingBasics.Infrastructure;

public static class DependencyInjection
{
  /// <summary>
  /// Registers Infrastructure layer services (DbContext, Repositories).
  /// </summary>
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<AdventureWorksDbContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();

    return services;
  }
}