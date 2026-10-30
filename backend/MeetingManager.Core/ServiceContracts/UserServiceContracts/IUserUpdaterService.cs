using MeetingManager.Core.DTO.UserDTOs;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserUpdaterService
    {
        Task GiveUserOrganizerRoleAsync(Guid userId, CancellationToken ct = default);
        Task RevokeUsersOrganizerRoleAsync(Guid userId, CancellationToken ct = default);

        Task<UserResponse> UpdateUserProfile(Guid userId, UserUpdateRequest request, CancellationToken ct = default);
    }
}
