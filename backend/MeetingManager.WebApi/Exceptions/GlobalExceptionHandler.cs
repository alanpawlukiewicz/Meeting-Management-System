using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MeetingManager.WebApi.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode < 500 ? exception.Message : "An unexpected server error occurred.",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title) MapException(Exception exception) =>
            exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                TimeoutException => (StatusCodes.Status504GatewayTimeout, "Timeout"),
                OperationCanceledException => (StatusCodes.Status499ClientClosedRequest, "Request cancelled"),

                _ => (StatusCodes.Status500InternalServerError, "A server error occurred")
            };
    }
}
