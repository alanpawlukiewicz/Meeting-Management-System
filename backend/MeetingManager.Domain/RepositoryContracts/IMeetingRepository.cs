using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IMeetingRepository : IRepository<Meeting, Guid>
    {
        Task<List<Meeting>> GetCurrentParticipantMeetingsAsync(Guid userId, CancellationToken ct = default);
        Task<Meeting?> FindMeetingForUpdateAsync(Guid meetingId, CancellationToken ct = default);
        Task<Meeting?> FindMeetingAsync(Guid meetingId, CancellationToken ct = default);
    }
}
