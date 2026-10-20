using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Configuration
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Secret { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpirationMinutes { get; init; }
    }
}
