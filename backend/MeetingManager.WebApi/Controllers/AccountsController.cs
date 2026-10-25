using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AccountsController : DefaultControllerBase
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICurrentUserService _currentUserService;
        public AccountsController(IUserGetterService userGetterService, ICurrentUserService currentUserService)
        {
            _userGetterService = userGetterService;
            _currentUserService = currentUserService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(CancellationToken ct)
        {
            return await _userGetterService.GetUserProfileAsync(_currentUserService.UserId, ct);
        }
    }
}
