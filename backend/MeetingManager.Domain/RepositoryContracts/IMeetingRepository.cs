using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IMeetingRepository : IRepository<Meeting, Guid>
    {
        Task<List<Meeting>> GetParticipantMeetingsAsync(Guid userId, CancellationToken ct = default);
    }
}
