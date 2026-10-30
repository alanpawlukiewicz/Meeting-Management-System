using MeetingManager.Core.DTO.UserDTOs;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.WebApi.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AccountsController : DefaultControllerBase
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserUpdaterService _userUpdaterService;
        private readonly IUserDeleterService _userDeleterService;
        public AccountsController(IUserGetterService userGetterService, ICurrentUserService currentUserService, IUserUpdaterService userUpdaterService, IUserDeleterService userDeleterService)
        {
            _userGetterService = userGetterService;
            _currentUserService = currentUserService;
            _userUpdaterService = userUpdaterService;
            _userDeleterService = userDeleterService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(CancellationToken ct)
        {
            return await _userGetterService.GetUserProfileAsync(_currentUserService.UserId, ct);
        }

        [HttpPut("me")]
        public async Task<ActionResult<UserResponse>> UpdateUserProfile(UserUpdateRequest request, CancellationToken ct)
        {
            return await _userUpdaterService.UpdateUserProfile(_currentUserService.UserId, request, ct);
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteUser(CancellationToken ct)
        {
            await _userDeleterService.DeleteUserAsync(_currentUserService.UserId, ct);

            Response.Cookies.Delete("refreshToken", CustomCookieOptions.RefreshCookieOptions());

            return NoContent();
        }
    }
}
