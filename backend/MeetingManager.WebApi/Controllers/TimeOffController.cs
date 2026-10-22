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
        private readonly ICurrentUserService _currentUserService;

        public TimeOffController(IUserTimeOffAdderService userTimeOffAdderService, ICurrentUserService currentUserService)
        {
            _userTimeOffAdderService = userTimeOffAdderService;
            _currentUserService = currentUserService;
        }

        [HttpPost("me")]
        public async Task<ActionResult<UserTimeOffResponse>> AddUserTimeOff(UserTimeOffAddRequest request, CancellationToken ct)
            => await _userTimeOffAdderService.AddUserTimeOffAsync(_currentUserService.UserId, request, ct);
    }
}
