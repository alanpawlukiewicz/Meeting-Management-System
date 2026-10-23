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
        private readonly IMeetingAttendanceUpdaterService _meetingAttendanceUpdaterService;

        public MeetingController(IMeetingGetterService meetingGetterService, ICurrentUserService currentUserService, IMeetingAttendanceUpdaterService meetingAttendanceUpdaterService)
        {
            _meetingGetterService = meetingGetterService;
            _currentUserService = currentUserService;
            _meetingAttendanceUpdaterService = meetingAttendanceUpdaterService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<List<MeetingDto>>> GetUserMeetings(CancellationToken ct)
            => await _meetingGetterService.GetUserMeetingsAsync(_currentUserService.UserId, ct);

        [HttpPut("{meetingId:guid}/attendance")]
        public async Task<IActionResult> ChangeAttendance(Guid meetingId, StatusDto request, CancellationToken ct)
        {
            await _meetingAttendanceUpdaterService.ChangeAttendanceAsync(meetingId, _currentUserService.UserId, request, ct);
            return NoContent();
        }
    }
}
