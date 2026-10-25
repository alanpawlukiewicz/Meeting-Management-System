using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts.MeetingServiceContracts
{
    public interface IMeetingDeleterService
    {
        Task DeleteMeetingAsync(Guid userId, Guid meetingId, CancellationToken ct = default);
    }
}
