using MeetingManager.Core.DTO;
using MeetingManager.Core.DTO.UserDTOs;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserRegistrationService
    {
        Task<AuthResponse> RegisterUserAsync(UserRegisterRequest request, string? ipAddress, string? userAgent, CancellationToken ct = default);
    }
}
