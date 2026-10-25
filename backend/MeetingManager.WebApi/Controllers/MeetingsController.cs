using MeetingManager.Core.Constants;
using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.ServiceContracts.MeetingServiceContracts;
using MeetingManager.Infrastructure.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class MeetingsController : DefaultControllerBase
    {
        private readonly IMeetingGetterService _meetingGetterService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMeetingAttendanceUpdaterService _meetingAttendanceUpdaterService;
        private readonly IMeetingAdderService _meetingAdderService;
        private readonly IMeetingUpdaterService _meetingUpdaterService;
        private readonly IMeetingDeleterService _meetingDeleterService;

        public MeetingsController(IMeetingGetterService meetingGetterService, ICurrentUserService currentUserService, IMeetingAttendanceUpdaterService meetingAttendanceUpdaterService, IMeetingAdderService meetingAdderService, IMeetingUpdaterService meetingUpdaterService, IMeetingDeleterService meetingDeleterService)
        {
            _meetingGetterService = meetingGetterService;
            _currentUserService = currentUserService;
            _meetingAttendanceUpdaterService = meetingAttendanceUpdaterService;
            _meetingAdderService = meetingAdderService;
            _meetingUpdaterService = meetingUpdaterService;
            _meetingDeleterService = meetingDeleterService;
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
        public async Task<ActionResult<MeetingResponse>> AddMeeting(MeetingRequest request, CancellationToken ct)
            => await _meetingAdderService.AddMeetingAsync(_currentUserService.UserId, request, ct);

        [HasRole(RoleName.Organizer)]
        [HttpPut("{meetingId:guid}")]
        public async Task<ActionResult<MeetingResponse>> UpdateMeeting(Guid meetingId, MeetingRequest request, CancellationToken ct)
            => await _meetingUpdaterService.UpdateMeetingAsync(meetingId, _currentUserService.UserId, request, ct);

        [HasRole(RoleName.Organizer)]
        [HttpDelete("{meetingId:guid}")]
        public async Task<IActionResult> DeleteMeeting(Guid meetingId, CancellationToken ct)
        {
            await _meetingDeleterService.DeleteMeetingAsync(_currentUserService.UserId, meetingId, ct);
            return NoContent();
        }
    }
}
