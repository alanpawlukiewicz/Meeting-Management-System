using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IMeetingParticipantRepository : IRepository<MeetingParticipant, Guid>
    {
        Task<MeetingParticipant?> FindMeetingParticipantByMeetingAndUserIdsAsync(Guid meetingId, Guid userId, CancellationToken ct = default);
    }
}
