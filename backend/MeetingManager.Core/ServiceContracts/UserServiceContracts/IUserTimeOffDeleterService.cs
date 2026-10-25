namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserTimeOffDeleterService
    {
        Task DeleteTimeOffAsync(Guid timeOffId, CancellationToken ct = default);
    }
}
