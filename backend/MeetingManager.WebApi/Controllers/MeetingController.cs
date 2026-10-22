using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class MeetingController : DefaultControllerBase
    {
        private readonly IMeetingGetterService _meetingGetterService;
        private readonly ICurrentUserService _currentUserService;

        public MeetingController(IMeetingGetterService meetingGetterService, ICurrentUserService currentUserService)
        {
            _meetingGetterService = meetingGetterService;
            _currentUserService = currentUserService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<List<MeetingDto>>> GetUserMeetings(CancellationToken ct)
            => await _meetingGetterService.GetUserMeetingsAsync(_currentUserService.UserId, ct);
    }
}
