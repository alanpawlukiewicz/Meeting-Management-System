using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> FindUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<User?> FindUserByLoginAsync(string login, CancellationToken ct = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.UserName == login, ct);
        }

        public async Task<User?> FindUserWithRolesAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsSplitQuery()
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<List<Guid>> GetAllUserIdsAsync(CancellationToken ct = default)
        {
            return await DbSet.Select(u => u.Id).ToListAsync(ct);
        }

        public async Task<List<User>> GetUsersWithAvailabilitiesAsync(List<Guid>? userIds, DateOnly date, CancellationToken ct = default)
        {
            if (userIds == null)
            {
                return await DbSet
                    .AsNoTracking()
                    .Include(u => u.UserTimeOffs.Where(uto => uto.Date == date))
                    .Include(u => u.UserAvailabilities.Where(ua => ua.DayOfWeek == date.DayOfWeek))
                    .AsSplitQuery()
                    .ToListAsync(ct);
            }

            return await DbSet
                   .AsNoTracking()
                   .Where(u => userIds.Contains(u.Id))
                   .Include(u => u.UserTimeOffs.Where(uto => uto.Date == date))
                   .Include(u => u.UserAvailabilities.Where(ua => ua.DayOfWeek == date.DayOfWeek))
                   .AsSplitQuery()
                   .ToListAsync(ct);
        }

        public async Task<List<User>> GetUsersWithLimitAsync(string query, int limit = 10, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(u => u.UserName.Contains(query)
                    || (u.FirstName ?? string.Empty).Contains(query)
                    || (u.LastName ?? string.Empty).Contains(query))
                .Take(limit)
                .ToListAsync(ct);
        }
    }
}
