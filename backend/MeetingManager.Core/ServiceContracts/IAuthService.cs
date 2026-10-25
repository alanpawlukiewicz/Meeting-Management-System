using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest request, string? ipAddress, string? userAgent, CancellationToken ct = default);

        Task<AuthResponse> RefreshTokensAsync(string? refreshToken, string? ipAddress, string? userAgent, CancellationToken ct = default);

        Task UpdateUserPasswordAsync(Guid userId, PasswordUpdateRequest request, CancellationToken ct = default);
    }
}
