using MeetingManager.Core.DTO.UserDTOs;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    [Route("api/time-offs")]
    public class TimeOffsController : DefaultControllerBase
    {
        private readonly IUserTimeOffAdderService _userTimeOffAdderService;
        private readonly IUserTimeOffDeleterService _userTimeOffDeleterService;
        private readonly ICurrentUserService _currentUserService;

        public TimeOffsController(IUserTimeOffAdderService userTimeOffAdderService, ICurrentUserService currentUserService, IUserTimeOffDeleterService userTimeOffDeleterService)
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
