using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AccountController : DefaultControllerBase
    {
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserGetterService _userGetterService;
        private readonly ICurrentUserService _currentUserService;
        public AccountController(IUserRegistrationService userRegistrationService, IUserGetterService userGetterService, ICurrentUserService currentUserService)
        {
            _userRegistrationService = userRegistrationService;
            _userGetterService = userGetterService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> RegisterUser(UserRegisterRequest request, CancellationToken ct)
            => await _userRegistrationService.RegisterUserAsync(request, ct);

        [HttpGet("me")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(CancellationToken ct)
        {
            return await _userGetterService.GetUserProfileAsync(_currentUserService.UserId, ct);
        }
    }
}
