using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.Services;
using MeetingManager.Infrastructure.Authorization.Handlers;
using MeetingManager.Infrastructure.Authorization.PolicyProviders;
using MeetingManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;

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

            services.AddScoped<IRoleAuthService, RoleAuthService>();
            services.AddSingleton<IAuthorizationPolicyProvider, RolePolicyProvider>();
            services.AddScoped<IAuthorizationHandler, RoleHandler>();

            // Background services
            services.AddHostedService<RefreshTokenCleanupService>();

            return services;
        }
    }
}
