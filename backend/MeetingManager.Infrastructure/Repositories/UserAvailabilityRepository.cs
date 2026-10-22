using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories
{
    public class UserAvailabilityRepository : Repository<UserAvailability, Guid>, IUserAvailabilityRepository
    {
        public UserAvailabilityRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserAvailability>> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet.AsNoTracking().Where(ua => ua.UserId == userId).ToListAsync(ct);
        }

        public async Task SetCurrentUserAvailabilitiesForDeleteAsync(Guid userId, CancellationToken ct = default)
        {
            var items = await DbSet.Where(ua => ua.UserId == userId).ToListAsync();
            DbSet.RemoveRange(items);
        }
    }
}
