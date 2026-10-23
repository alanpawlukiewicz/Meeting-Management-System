using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Repositories
{
    public class MeetingRepository : Repository<Meeting, Guid>, IMeetingRepository
    {
        public MeetingRepository(AppDbContext context) : base(context)
        {
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
