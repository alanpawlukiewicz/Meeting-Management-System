using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class UserController : DefaultControllerBase
    {
        private readonly IUserGetterService _userGetterService;

        public UserController(IUserGetterService userGetterService)
        {
            _userGetterService = userGetterService;
        }

        [HttpGet("availabilities")]
        public async Task<ActionResult<List<UserStatusCheckResponse>>> CheckUserAvailabilities(UserStatusCheckRequest request, CancellationToken ct)
            => await _userGetterService.CheckUserAvailabilitiesAsync(request, ct);
    }
}
