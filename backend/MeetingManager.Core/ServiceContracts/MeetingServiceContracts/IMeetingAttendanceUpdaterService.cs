using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.MeetingServiceContracts
{
    public interface IMeetingAttendanceUpdaterService
    {
        Task ChangeAttendanceAsync(Guid meetingId, Guid userId, StatusDto request, CancellationToken ct = default);
    }
}
