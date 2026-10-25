using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IRefreshTokenRotationService
    {
        Task RotateAsync(RefreshToken oldToken, RefreshTokenAddRequest newToken, CancellationToken ct = default);
    }
}
