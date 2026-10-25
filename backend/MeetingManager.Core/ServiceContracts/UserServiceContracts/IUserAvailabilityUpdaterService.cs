using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserAvailabilityUpdaterService
    {
        Task<List<UserAvailabilityDto>> UpdateUserAvailabilityListAsync(Guid userId, List<UserAvailabilityDto> requestList, CancellationToken ct = default);
    }
}
