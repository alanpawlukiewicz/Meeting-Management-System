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

        public async Task<List<Meeting>> GetParticipantMeetingsAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(m => m.MeetingParticipants.Any(mp => mp.UserId == userId))
                .Include(m => m.User)
                .ToListAsync(ct);
        }
    }
}
