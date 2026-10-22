using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    [Route("api/time-off")]
    public class TimeOffController : DefaultControllerBase
    {
        private readonly IUserTimeOffAdderService _userTimeOffAdderService;
        private readonly IUserTimeOffDeleterService _userTimeOffDeleterService;
        private readonly ICurrentUserService _currentUserService;

        public TimeOffController(IUserTimeOffAdderService userTimeOffAdderService, ICurrentUserService currentUserService, IUserTimeOffDeleterService userTimeOffDeleterService)
        {
            _userTimeOffAdderService = userTimeOffAdderService;
            _currentUserService = currentUserService;
            _userTimeOffDeleterService = userTimeOffDeleterService;
        }

        [HttpPost("me")]
        public async Task<ActionResult<UserTimeOffResponse>> AddUserTimeOff(UserTimeOffAddRequest request, CancellationToken ct)
            => await _userTimeOffAdderService.AddUserTimeOffAsync(_currentUserService.UserId, request, ct);

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> AddUserTimeOff(Guid id, CancellationToken ct)
        {
            await _userTimeOffDeleterService.DeleteTimeOffAsync(id, ct);
            return NoContent();
        }
    }
}
