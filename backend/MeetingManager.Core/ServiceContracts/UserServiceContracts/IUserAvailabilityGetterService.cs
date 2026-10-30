using MeetingManager.Core.DTO.UserDTOs;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserAvailabilityGetterService
    {
        Task<UserAvailabilityDetails> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default);
    }
}
