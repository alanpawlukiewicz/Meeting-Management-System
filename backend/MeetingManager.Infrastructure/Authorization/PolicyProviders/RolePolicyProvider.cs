using MeetingManager.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MeetingManager.Infrastructure.Authorization.PolicyProviders
{
    public class RolePolicyProvider : IAuthorizationPolicyProvider
    {
        private const string Prefix = "Role:";
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public RolePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            {
                var roleName = policyName[Prefix.Length..];
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new RoleRequirement(roleName))
                    .Build();
                return Task.FromResult<AuthorizationPolicy?>(policy);
            }
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
