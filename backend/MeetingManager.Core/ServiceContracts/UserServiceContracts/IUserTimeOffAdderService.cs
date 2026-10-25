using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserTimeOffAdderService
    {
        Task<UserTimeOffResponse> AddUserTimeOffAsync(Guid userId, UserTimeOffAddRequest request, CancellationToken ct = default);
    }
}
