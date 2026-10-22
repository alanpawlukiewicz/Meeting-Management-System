using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.Services;
using MeetingManager.Infrastructure.Services;

namespace MeetingManager.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserGetterService, UserGetterService>();

            services.AddScoped<IUserAvailabilityUpdaterService, UserAvailabilityUpdaterService>();
            services.AddScoped<IUserAvailabilityGetterService, UserAvailabilityGetterService>();

            services.AddScoped<IUserTimeOffAdderService, UserTimeOffAdderService>();
            services.AddScoped<IUserTimeOffDeleterService, UserTimeOffDeleterService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IRefreshTokenAdderService, RefreshTokenAdderService>();
            services.AddScoped<IRefreshTokenDeleterService, RefreshTokenDeleterService>();
            services.AddScoped<IRefreshTokenRotationService, RefreshTokenRotationService>();

            services.AddScoped<IJwtService, JwtService>();

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            services.AddSingleton<ITokenHasher, Sha256TokenHasher>();

            // Background services
            services.AddHostedService<RefreshTokenCleanupService>();

            return services;
        }
    }
}
