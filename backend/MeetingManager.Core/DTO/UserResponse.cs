using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
