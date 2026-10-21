using MeetingManager.Infrastructure.Configuration;

namespace MeetingManager.WebApi.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>()!;
            services.AddCors(options =>
            {
                options.AddPolicy(corsOptions.FrontendCorsName,
                    corsbuilder =>
                    {
                        corsbuilder.WithOrigins(corsOptions.FrontendBaseUrl)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
