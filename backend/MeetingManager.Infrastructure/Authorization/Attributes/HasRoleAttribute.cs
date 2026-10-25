using Microsoft.AspNetCore.Authorization;

namespace MeetingManager.Infrastructure.Authorization.Attributes
{
    public class HasRoleAttribute : AuthorizeAttribute
    {
        public HasRoleAttribute(string roleName) : base(policy: $"Role:{roleName}") { }
    }
}
