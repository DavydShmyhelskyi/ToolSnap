using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Services;
using Application.Common.Settings;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.BlobStorage;
using Infrastructure.Persistence;
using Infrastructure.Services;
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
        services.AddNotificationServices();
        services.AddBackgroundJobs();
    }

    private static void AddFileStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, LocalFileStorage>();
    }

    private static void AddNotificationServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
    }

    private static void AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<DeadlineCheckerJob>();
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