using MeetingManager.Infrastructure.Configuration;

namespace MeetingManager.WebApi.Extensions
{
    public static class OptionExtensions
    {
        public static IServiceCollection AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<RefreshTokenOptions>(configuration.GetSection(RefreshTokenOptions.SectionName));

            return services;
        }
    }
}
