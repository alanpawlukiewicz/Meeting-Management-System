using Microsoft.AspNetCore.Authorization;

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
