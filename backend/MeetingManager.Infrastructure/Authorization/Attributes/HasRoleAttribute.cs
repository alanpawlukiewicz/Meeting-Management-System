using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Authorization.Attributes
{
    public class HasRoleAttribute : AuthorizeAttribute
    {
        public HasRoleAttribute(string roleName) : base(policy: $"Role:{roleName}") { }
    }
}
