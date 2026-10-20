using MeetingManager.Core.DTO;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserRegistrationService
    {
        Task<UserDto> RegisterUserAsync(UserRegisterRequest request, CancellationToken ct = default);
    }
}
