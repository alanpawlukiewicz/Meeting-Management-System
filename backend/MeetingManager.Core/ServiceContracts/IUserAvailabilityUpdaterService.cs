using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserAvailabilityUpdaterService
    {
        Task<List<UserAvailabilityDto>> UpdateUserAvailabilityListAsync(Guid userId, List<UserAvailabilityDto> requestList, CancellationToken ct = default);
    }
}
