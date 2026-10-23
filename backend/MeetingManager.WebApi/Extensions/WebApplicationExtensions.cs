using MeetingManager.Infrastructure.Configuration;
using Microsoft.AspNetCore.HttpOverrides;

namespace MeetingManager.WebApi.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseApiPipeline(this WebApplication app)
        {
            var corsOptions = app.Configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>()!;

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseExceptionHandler();

            app.UseRouting();

            app.UseCors(corsOptions.FrontendCorsName);

            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCustomLogging();

            app.MapControllers();

            return app;
        }
    }
}
