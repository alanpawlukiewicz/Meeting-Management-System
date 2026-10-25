using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.MeetingServiceContracts
{
    public interface IMeetingUpdaterService
    {
        Task<MeetingResponse> UpdateMeetingAsync(Guid meetingId, Guid creatorId, MeetingRequest request, CancellationToken ct = default);
    }
}
