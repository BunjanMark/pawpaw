using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PawPaw.Infrastructure.Data;
using PawPaw.Infrastructure.Storage;
using PawPaw.UseCases.Files;

namespace PawPaw.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection")
      ?? throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection is required. Configure it with dotnet user-secrets locally.");

    services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
    services.Configure<LocalStorageOptions>(configuration.GetSection(LocalStorageOptions.SectionName));
    services.AddScoped<IFileStorage, LocalFileStorage>();

    return services;
  }
}
