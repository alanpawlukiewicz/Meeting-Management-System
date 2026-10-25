using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserAvailabilityGetterService
    {
        Task<UserAvailabilityDetails> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default);
    }
}
