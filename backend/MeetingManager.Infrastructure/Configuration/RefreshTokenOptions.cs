using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Configuration
{
    public class RefreshTokenOptions
    {
        public const string SectionName = "RefreshToken";

        public int ExpirationMinutes { get; init; }
    }
}
