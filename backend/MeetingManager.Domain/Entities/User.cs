using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
