using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserGetterService
    {
        Task<UserProfile> GetUserProfileAsync(Guid userId, CancellationToken ct = default);

        Task<List<UserStatusCheckResponse>> CheckUserAvailabilitiesAsync(UserStatusCheckRequest request, CancellationToken ct = default);

        Task<List<LookupDto>> GetUsersForLookupAsync(string query, int limit = 10, CancellationToken ct = default);
        Task<List<UserDto>> GetAllUsersWithRolesAsync(CancellationToken ct = default);
        Task<bool> CheckUserNameAvailbility(string username, CancellationToken ct = default);
    }
}
