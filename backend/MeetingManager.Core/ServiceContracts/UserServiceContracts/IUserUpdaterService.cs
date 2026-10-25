namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserUpdaterService
    {
        Task GiveUserOrganizerRoleAsync(Guid userId, CancellationToken ct = default);
        Task RevokeUsersOrganizerRoleAsync(Guid userId, CancellationToken ct = default);
    }
}
