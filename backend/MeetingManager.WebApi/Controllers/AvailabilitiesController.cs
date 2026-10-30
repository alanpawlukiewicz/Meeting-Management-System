using MeetingManager.Core.DTO.UserDTOs;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AvailabilitiesController : DefaultControllerBase
    {
        private readonly IUserAvailabilityUpdaterService _userAvailabilityUpdaterService;
        private readonly IUserAvailabilityGetterService _userAvailabilityGetterService;
        private readonly ICurrentUserService _currentUserService;

        public AvailabilitiesController(IUserAvailabilityUpdaterService userAvailabilityUpdaterService, ICurrentUserService currentUserService, IUserAvailabilityGetterService userAvailabilityGetterService)
        {
            _userAvailabilityUpdaterService = userAvailabilityUpdaterService;
            _currentUserService = currentUserService;
            _userAvailabilityGetterService = userAvailabilityGetterService;
        }

        [HttpPut("me")]
        public async Task<ActionResult<List<UserAvailabilityDto>>> UpdateUserAvailability(List<UserAvailabilityDto> requestList, CancellationToken ct)
            => await _userAvailabilityUpdaterService.UpdateUserAvailabilityListAsync(_currentUserService.UserId, requestList, ct);

        [HttpGet("me")]
        public async Task<ActionResult<UserAvailabilityDetails>> GetUserAvailabilitiesWithTimeOffs(CancellationToken ct)
            => await _userAvailabilityGetterService.GetUserAvailabilitiesAsync(_currentUserService.UserId, ct);
    }
}
