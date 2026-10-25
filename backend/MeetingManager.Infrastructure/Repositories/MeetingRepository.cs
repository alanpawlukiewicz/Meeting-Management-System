using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories
{
    public class MeetingRepository : Repository<Meeting, Guid>, IMeetingRepository
    {
        public MeetingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Meeting?> FindMeetingForUpdateAsync(Guid meetingId, CancellationToken ct = default)
        {
            return await DbSet
                .Where(m => m.Id == meetingId)
                .Include(m => m.MeetingParticipants)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<Meeting>> GetCurrentParticipantMeetingsAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet
                    .AsNoTracking()
                    .Where(m => m.MeetingParticipants.Any(mp => mp.UserId == userId)
                    && m.Date >= DateOnly.FromDateTime(DateTime.UtcNow))
                    .Include(m => m.User)
                    .Include(m => m.MeetingParticipants.Where(mp => mp.UserId == userId))
                    .ToListAsync(ct);
        }
    }
}
