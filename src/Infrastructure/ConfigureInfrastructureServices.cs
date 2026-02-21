using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Settings;
using Infrastructure.Persistence;
using Infrastructure.BlobStorage;
using Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);
        services.AddFileStorageServices();
        services.AddAuthenticationServices(configuration);
    }

    private static void AddFileStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, LocalFileStorage>();
    }

    private static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT Settings - bind from appsettings.json
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
        
        if (string.IsNullOrEmpty(jwtSettings.Secret))
            throw new InvalidOperationException("JWT settings are not configured in appsettings.json");
        
        services.AddSingleton(jwtSettings);
        
        // JWT Token Generator
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
    }
}