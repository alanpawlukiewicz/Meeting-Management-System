using MeetingManager.Domain.Enums;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.WebApi.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MapEnum<AttendanceStatus>("attendance_status");
                    })
                .UseSnakeCaseNamingConvention();
            });

            return services;
        }
    }
}
