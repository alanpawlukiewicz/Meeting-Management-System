using MeetingManager.Core.Constants;
using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Infrastructure.Authorization.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class UserController : DefaultControllerBase
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IUserUpdaterService _userUpdaterService;

        public UserController(IUserGetterService userGetterService, IUserUpdaterService userUpdaterService)
        {
            _userGetterService = userGetterService;
            _userUpdaterService = userUpdaterService;
        }

        [HttpPost("availabilities")]
        public async Task<ActionResult<List<UserStatusCheckResponse>>> CheckUserAvailabilities(UserStatusCheckRequest request, CancellationToken ct)
            => await _userGetterService.CheckUserAvailabilitiesAsync(request, ct);

        [HttpGet("lookup")]
        public async Task<ActionResult<List<LookupDto>>> GetUsersForLookup(
            [FromQuery] string q,
            [FromQuery] int? limit,
            CancellationToken ct)
            => await _userGetterService.GetUsersForLookupAsync(q, limit ?? 10, ct);

        [HasRole(RoleName.Admin)]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers(CancellationToken ct)
            => await _userGetterService.GetAllUsersWithRolesAsync(ct);

        [HasRole(RoleName.Admin)]
        [HttpPut("{userId:guid}/roles/organizer")]
        public async Task<IActionResult> GiveUserOrganizerRole(Guid userId, CancellationToken ct)
        {
            await _userUpdaterService.GiveUserOrganizerRoleAsync(userId, ct);
            return NoContent();
        }

        [HasRole(RoleName.Admin)]
        [HttpDelete("{userId:guid}/roles/organizer")]
        public async Task<IActionResult> RevokeUsersOrganizerRole(Guid userId, CancellationToken ct)
        {
            await _userUpdaterService.RevokeUsersOrganizerRoleAsync(userId, ct);
            return NoContent();
        }
    }
}
