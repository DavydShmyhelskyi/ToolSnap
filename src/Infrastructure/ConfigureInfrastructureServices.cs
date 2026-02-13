using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.BlobStorage;

namespace Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);
        services.AddFileStorageServices();
    }

    private static void AddFileStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, LocalFileStorage>();
    }
}