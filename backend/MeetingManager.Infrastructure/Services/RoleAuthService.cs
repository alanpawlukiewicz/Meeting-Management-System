using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Services
{
    public class RoleAuthService : IRoleAuthService
    {
        private readonly IMemoryCache _cache;
        private readonly IRoleRepository _roleRepository;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public RoleAuthService(IMemoryCache cache, IRoleRepository roleRepository)
        {
            _cache = cache;
            _roleRepository = roleRepository;
        }

        private async Task<HashSet<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
        {
            var cacheKey = $"perms:{userId}";
            if (_cache.TryGetValue(cacheKey, out HashSet<string>? cached))
                return cached!;

            var roles = await _roleRepository.GetUserRolesAsync(userId, ct);

            var rolehs = roles.Select(r => r.Name).ToHashSet();
            _cache.Set(cacheKey, rolehs, CacheDuration);
            return rolehs;

        }

        public async Task<bool> HasRoleAsync(Guid userId, string roleName, CancellationToken ct = default)
        {
            var userRoleNames = await GetUserRolesAsync(userId, ct);

            return userRoleNames.Contains(roleName);
        }
    }
}
