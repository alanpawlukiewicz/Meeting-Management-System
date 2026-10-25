using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Events;

namespace MeetingManager.WebApi.Extensions
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestProperties
                | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }

        public static IHostBuilder AddSerilogLogging(this IHostBuilder host)
        {
            host.UseSerilog((hostingContext, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .ReadFrom.Services(services);
            });

            return host;
        }

        public static WebApplication UseCustomLogging(this WebApplication app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = (httpContext, elapsed, ex) =>
                {
                    if (ex != null || httpContext.Response.StatusCode >= 500)
                        return LogEventLevel.Error;

                    if (httpContext.Request.Method == "GET" && elapsed < 500)
                        return LogEventLevel.Debug;

                    return LogEventLevel.Information;
                };
            });

            return app;
        }
    }
}
