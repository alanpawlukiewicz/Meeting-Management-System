using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserAvailabilityGetterService
    {
        Task<List<UserAvailabilityDto>> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default);
    }
}
