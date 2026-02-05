using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // Locations
        services.AddScoped<LocationsRepository>();
        services.AddScoped<ILocationRepository>(provider => provider.GetRequiredService<LocationsRepository>());
        services.AddScoped<LocationsQueries>();
        services.AddScoped<ILocationsQueries>(provider => provider.GetRequiredService<LocationsQueries>());

        // LocationTypes
        services.AddScoped<LocationTypeRepository>();
        services.AddScoped<ILocationTypeRepository>(provider => provider.GetRequiredService<LocationTypeRepository>());
        services.AddScoped<LocationTypeQueries>();
        services.AddScoped<ILocationTypeQueries>(provider => provider.GetRequiredService<LocationTypeQueries>());

        // Tools
        services.AddScoped<ToolsRepository>();
        services.AddScoped<IToolsRepository>(provider => provider.GetRequiredService<ToolsRepository>());
        services.AddScoped<ToolsQueries>();
        services.AddScoped<IToolsQueries>(provider => provider.GetRequiredService<ToolsQueries>());

        // ToolStatuses
        services.AddScoped<ToolStatusRepository>();
        services.AddScoped<IToolStatusRepository>(provider => provider.GetRequiredService<ToolStatusRepository>());
        services.AddScoped<ToolStatusQueries>();
        services.AddScoped<IToolStatusQueries>(provider => provider.GetRequiredService<ToolStatusQueries>());

        // ToolPhotos
        services.AddScoped<ToolPhotosRepository>();
        services.AddScoped<IToolPhotosRepository>(provider => provider.GetRequiredService<ToolPhotosRepository>());
        services.AddScoped<ToolPhotosQueries>();
        services.AddScoped<IToolPhotosQueries>(provider => provider.GetRequiredService<ToolPhotosQueries>());

        // PhotoTypes
        services.AddScoped<PhotoTypeRepository>();
        services.AddScoped<IPhotoTypeRepository>(provider => provider.GetRequiredService<PhotoTypeRepository>());
        services.AddScoped<PhotoTypeQueries>();
        services.AddScoped<IPhotoTypeQueries>(provider => provider.GetRequiredService<PhotoTypeQueries>());

        // ToolAssignments
        services.AddScoped<ToolAssignmentsRepository>();
        services.AddScoped<IToolAssignmentsRepository>(provider => provider.GetRequiredService<ToolAssignmentsRepository>());
        services.AddScoped<ToolAssignmentQueries>();
        services.AddScoped<IToolAssignmentQueries>(provider => provider.GetRequiredService<ToolAssignmentQueries>());

        // Users
        services.AddScoped<UsersRepository>();
        services.AddScoped<IUsersRepository>(provider => provider.GetRequiredService<UsersRepository>());
        services.AddScoped<UsersQueries>();
        services.AddScoped<IUsersQueries>(provider => provider.GetRequiredService<UsersQueries>());

        // Roles
        services.AddScoped<RolesRepository>();
        services.AddScoped<IRolesRepository>(provider => provider.GetRequiredService<RolesRepository>());
        services.AddScoped<RolesQueries>();
        services.AddScoped<IRolesQueries>(provider => provider.GetRequiredService<RolesQueries>());

        // ActionTypes
        services.AddScoped<ActionTypeRepository>();
        services.AddScoped<IActionTypeRepository>(provider => provider.GetRequiredService<ActionTypeRepository>());
        services.AddScoped<ActionTypeQueries>();
        services.AddScoped<IActionTypeQueries>(provider => provider.GetRequiredService<ActionTypeQueries>());

        // Brands
        services.AddScoped<BrandRepository>();
        services.AddScoped<IBrandRepository>(provider => provider.GetRequiredService<BrandRepository>());
        services.AddScoped<BrandQueries>();
        services.AddScoped<IBrandQueries>(provider => provider.GetRequiredService<BrandQueries>());

        // Models
        services.AddScoped<ModelRepository>();
        services.AddScoped<IModelRepository>(provider => provider.GetRequiredService<ModelRepository>());
        services.AddScoped<ModelQueries>();
        services.AddScoped<IModelQueries>(provider => provider.GetRequiredService<ModelQueries>());

        // ToolTypes
        services.AddScoped<ToolTypeRepository>();
        services.AddScoped<IToolTypeRepository>(provider => provider.GetRequiredService<ToolTypeRepository>());
        services.AddScoped<ToolTypeQueries>();
        services.AddScoped<IToolTypeQueries>(provider => provider.GetRequiredService<ToolTypeQueries>());

        // PhotoSessions
        services.AddScoped<PhotoSessionsRepository>();
        services.AddScoped<IPhotoSessionsRepository>(provider => provider.GetRequiredService<PhotoSessionsRepository>());
        services.AddScoped<PhotoSessionsQueries>();
        services.AddScoped<IPhotoSessionsQueries>(provider => provider.GetRequiredService<PhotoSessionsQueries>());

        // PhotosForDetection
        services.AddScoped<PhotoForDetectionRepository>();
        services.AddScoped<IPhotoForDetectionRepository>(provider => provider.GetRequiredService<PhotoForDetectionRepository>());
        services.AddScoped<PhotoForDetectionQueries>();
        services.AddScoped<IPhotoForDetectionQueries>(provider => provider.GetRequiredService<PhotoForDetectionQueries>());

        // DetectedTools
        services.AddScoped<DetectedToolRepository>();
        services.AddScoped<IDetectedToolRepository>(provider => provider.GetRequiredService<DetectedToolRepository>());
        services.AddScoped<DetectedToolQueries>();
        services.AddScoped<IDetectedToolQueries>(provider => provider.GetRequiredService<DetectedToolQueries>());
    }
}