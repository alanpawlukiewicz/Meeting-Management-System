using MeetingManager.Core.ServiceContracts;
using MeetingManager.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace MeetingManager.Infrastructure.Authorization.Handlers
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IRoleAuthService _roleAuthService;
        private readonly ICurrentUserService _currentUserService;

        public RoleHandler(IRoleAuthService roleAuthService, ICurrentUserService currentUserService)
        {
            _roleAuthService = roleAuthService;
            _currentUserService = currentUserService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (await _roleAuthService.HasRoleAsync(_currentUserService.UserId, requirement.RoleName))
                context.Succeed(requirement);
        }
    }
}
