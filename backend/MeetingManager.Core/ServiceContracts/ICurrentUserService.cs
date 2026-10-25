namespace MeetingManager.Core.ServiceContracts
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
