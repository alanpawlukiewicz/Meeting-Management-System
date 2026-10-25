namespace MeetingManager.Core.ServiceContracts
{
    public interface IRefreshTokenDeleterService
    {
        Task DeleteTokenAsync(string token, CancellationToken ct = default);
    }
}
