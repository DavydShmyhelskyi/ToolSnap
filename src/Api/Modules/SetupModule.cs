using Api.Filters;
using Application.Common.Settings;
using FluentValidation;
using Api.Services.Abstract;
using Api.Services.Implementation;

namespace Api.Modules
{
    public static class SetupModule
    {
        public static void SetupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            services.AddCors();
            services.AddRequestValidators();
            services.AddApplicationSettings(configuration);
            services.AddControllerServices();
        }


        public static void AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.SetIsOriginAllowed(_ => true)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials())); 
        }


        private static void AddRequestValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
        }


        private static void AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<ApplicationSettings>();
            if (settings != null)
            {
                services.AddSingleton(settings);
            }
        }



        private static void AddControllerServices(this IServiceCollection services)
        {
            services.AddScoped<IActionTypeControllerService, ActionTypeControllerService>();
            services.AddScoped<IBrandControllerService, BrandControllerService>();          
            services.AddScoped<IDetectedToolControllerService, DetectedToolControllerService>();
            services.AddScoped<ILocationControllerService, LocationControllerService>();
            services.AddScoped<ILocationTypeControllerService, LocationTypeControllerService>();
            services.AddScoped<IModelControllerService, ModelControllerService>();
            services.AddScoped<IPhotoForDetectionControllerService, PhotoForDetectionControllerService>();
            services.AddScoped<IPhotoSessionControllerService, PhotoSessionControllerService>();
            services.AddScoped<IPhotoTypeControllerService, PhotoTypeControllerService>();
            services.AddScoped<IRoleControllerService, RoleControllerService>();
            services.AddScoped<IToolAssignmentControllerService, ToolAssignmentControllerService>();
            services.AddScoped<IToolControllerService, ToolControllerService>();
            services.AddScoped<IToolPhotoControllerService, ToolPhotoControllerService>();
            services.AddScoped<IToolStatusControllerService, ToolStatusControllerService>();
            services.AddScoped<IToolTypeControllerService, ToolTypeControllerService>();
            services.AddScoped<IUserControllerService, UserControllerService>();
        }
    }
}
