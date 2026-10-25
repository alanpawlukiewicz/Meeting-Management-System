using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.MeetingServiceContracts
{
    public interface IMeetingAdderService
    {
        Task<MeetingResponse> AddMeetingAsync(Guid creatorId, MeetingRequest request, CancellationToken ct = default);
    }
}
