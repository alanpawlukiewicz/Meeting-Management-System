using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthResponse> GenerateJwtToken(User user, CancellationToken ct = default);
    }
}
