using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
