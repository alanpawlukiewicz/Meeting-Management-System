using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.Repositories;

namespace MeetingManager.WebApi.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserAvailabilityRepository, UserAvailabilityRepository>();
            services.AddScoped<IUserTimeOffRepository, UserTimeOffRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IMeetingParticipantRepository, MeetingParticipantRepository>();

            return services;
        }
    }
}
