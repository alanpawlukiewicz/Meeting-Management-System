using MeetingManager.Core.DTO.MeetingDtos;

namespace MeetingManager.Core.ServiceContracts.MeetingServiceContracts
{
    public interface IMeetingGetterService
    {
        Task<List<MeetingDto>> GetCurrentUserMeetingsAsync(Guid userId, CancellationToken ct = default);
    }
}
