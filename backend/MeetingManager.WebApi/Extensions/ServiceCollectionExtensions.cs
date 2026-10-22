using MeetingManager.WebApi.Exceptions;

namespace MeetingManager.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddWebApiServices(configuration)
                .AddSecurityServices(configuration)
                .AddDataServices(configuration)
                .AddApplicationLayerServices();
        }

        private static IServiceCollection AddWebApiServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddConfigOptions(configuration);
            services.AddCorsConfiguration(configuration);
            return services;
        }

        private static IServiceCollection AddSecurityServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuth(configuration);
            services.AddCustomRateLimiter();

            return services;
        }

        private static IServiceCollection AddDataServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomDbContext(configuration);
            services.AddMemoryCache();
            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
        {
            services.AddServices();

            return services;
        }
    }
}
