using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Repositories
{
    public class MeetingParticipantRepository : Repository<MeetingParticipant, Guid>, IMeetingParticipantRepository
    {
        public MeetingParticipantRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MeetingParticipant?> FindMeetingParticipantByMeetingAndUserIdsAsync(Guid meetingId, Guid userId, CancellationToken ct = default)
        {
            return await DbSet.FirstOrDefaultAsync(mp => mp.MeetingId == meetingId && mp.UserId == userId, ct);
        }
    }
}
