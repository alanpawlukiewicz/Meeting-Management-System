using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AvailabilityController : DefaultControllerBase
    {
        private readonly IUserAvailabilityUpdaterService _userAvailabilityUpdaterService;
        private readonly IUserAvailabilityGetterService _userAvailabilityGetterService;
        private readonly ICurrentUserService _currentUserService;

        public AvailabilityController(IUserAvailabilityUpdaterService userAvailabilityUpdaterService, ICurrentUserService currentUserService, IUserAvailabilityGetterService userAvailabilityGetterService)
        {
            _userAvailabilityUpdaterService = userAvailabilityUpdaterService;
            _currentUserService = currentUserService;
            _userAvailabilityGetterService = userAvailabilityGetterService;
        }

        [HttpPut("me")]
        public async Task<ActionResult<List<UserAvailabilityDto>>> UpdateUserAvailability(List<UserAvailabilityDto> requestList, CancellationToken ct)
            => await _userAvailabilityUpdaterService.UpdateUserAvailabilityListAsync(_currentUserService.UserId, requestList, ct);

        [HttpGet("me")]
        public async Task<ActionResult<List<UserAvailabilityDto>>> GetUserAvailabilities(CancellationToken ct)
            => await _userAvailabilityGetterService.GetUserAvailabilitiesAsync(_currentUserService.UserId, ct);
    }
}
