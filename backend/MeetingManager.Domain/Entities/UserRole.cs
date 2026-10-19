using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; } = default!;
        public virtual Role Role { get; set; } = default!;
    }
}
