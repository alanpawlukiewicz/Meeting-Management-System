using MeetingManager.Core.DTO.UserDTOs;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserAvailabilityUpdaterService
    {
        Task<List<UserAvailabilityDto>> UpdateUserAvailabilityListAsync(Guid userId, List<UserAvailabilityDto> requestList, CancellationToken ct = default);
    }
}
