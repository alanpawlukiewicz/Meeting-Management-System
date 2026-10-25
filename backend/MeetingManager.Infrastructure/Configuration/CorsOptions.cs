namespace MeetingManager.Infrastructure.Configuration
{
    public class CorsOptions
    {
        public const string SectionName = "Cors";

        public string FrontendBaseUrl { get; init; } = null!;
        public string FrontendCorsName { get; init; } = null!;
    }
}
