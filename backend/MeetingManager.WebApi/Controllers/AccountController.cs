using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AccountController : DefaultControllerBase
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public AccountController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> RegisterUser(UserRegisterRequest request, CancellationToken ct)
            => await _userRegistrationService.RegisterUserAsync(request, ct);
    }
}
