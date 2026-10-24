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

        [HttpPost("availabilities")]
        public async Task<ActionResult<List<UserStatusCheckResponse>>> CheckUserAvailabilities(UserStatusCheckRequest request, CancellationToken ct)
            => await _userGetterService.CheckUserAvailabilitiesAsync(request, ct);

        [HttpGet]
        public async Task<ActionResult<List<LookupDto>>> GetUsers(
            [FromQuery] string query,
            [FromQuery] int limit,
            CancellationToken ct)
            => await _userGetterService.GetUsersForLookupAsync(query, limit, ct);
    }
}
