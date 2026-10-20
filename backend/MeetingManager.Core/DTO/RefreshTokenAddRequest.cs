using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class RefreshTokenAddRequest
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public DateTimeOffset ExpiresAt { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
    }
}
