using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class JwtTokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
