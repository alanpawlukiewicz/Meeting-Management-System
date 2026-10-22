using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; } = null!;

        public RoleRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
