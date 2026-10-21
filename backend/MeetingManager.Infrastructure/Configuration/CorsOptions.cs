using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Configuration
{
    public class CorsOptions
    {
        public const string SectionName = "Cors";

        public string FrontendBaseUrl { get; init; } = null!;
        public string FrontendCorsName { get; init; } = null!;
    }
}
