using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserRegistrationService
    {
        Task<AuthResponse> RegisterUserAsync(UserRegisterRequest request, string? ipAddress, string? userAgent, CancellationToken ct = default);
    }
}
