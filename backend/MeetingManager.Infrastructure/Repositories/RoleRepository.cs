using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role?> FindRoleByNameAsync(string name, CancellationToken ct = default)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(r => r.Name == name, ct);
        }

        public async Task<List<Role>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet
                    .AsNoTracking()
                    .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
                    .ToListAsync(ct);
        }
    }
}
