using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IRefreshTokenAdderService
    {
        Task AddTokenAsync(RefreshTokenAddRequest request, CancellationToken ct = default);
    }
}
