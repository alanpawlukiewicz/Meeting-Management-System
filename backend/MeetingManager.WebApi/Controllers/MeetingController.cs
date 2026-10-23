using MeetingManager.Core.Constants;
using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Infrastructure.Authorization.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class MeetingController : DefaultControllerBase
    {
        private readonly IMeetingGetterService _meetingGetterService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMeetingAttendanceUpdaterService _meetingAttendanceUpdaterService;
        private readonly IMeetingAdderService _meetingAdderService;

        public MeetingController(IMeetingGetterService meetingGetterService, ICurrentUserService currentUserService, IMeetingAttendanceUpdaterService meetingAttendanceUpdaterService, IMeetingAdderService meetingAdderService)
        {
            _meetingGetterService = meetingGetterService;
            _currentUserService = currentUserService;
            _meetingAttendanceUpdaterService = meetingAttendanceUpdaterService;
            _meetingAdderService = meetingAdderService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<List<MeetingDto>>> GetCurrentUserMeetings(CancellationToken ct)
            => await _meetingGetterService.GetCurrentUserMeetingsAsync(_currentUserService.UserId, ct);

        [HttpPut("{meetingId:guid}/attendance")]
        public async Task<IActionResult> ChangeAttendance(Guid meetingId, StatusDto request, CancellationToken ct)
        {
            await _meetingAttendanceUpdaterService.ChangeAttendanceAsync(meetingId, _currentUserService.UserId, request, ct);
            return NoContent();
        }

        [HasRole(RoleName.Organizer)]
        [HttpPost]
        public async Task<ActionResult<MeetingResponse>> AddMeeting(MeetingAddRequest request, CancellationToken ct)
            => await _meetingAdderService.AddMeetingAsync(_currentUserService.UserId, request, ct);
    }
}
