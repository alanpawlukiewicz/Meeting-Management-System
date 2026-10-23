using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IMeetingParticipantRepository : IRepository<MeetingParticipant, Guid>
    {
        Task<MeetingParticipant?> FindMeetingParticipantByMeetingAndUserIdsAsync(Guid meetingId, Guid userId, CancellationToken ct = default);
    }
}
