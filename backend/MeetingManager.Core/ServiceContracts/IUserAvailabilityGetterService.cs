using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserAvailabilityGetterService
    {
        Task<UserAvailabilityDetails> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default);
    }
}
