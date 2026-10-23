using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IMeetingUpdaterService
    {
        Task<MeetingResponse> UpdateMeetingAsync(Guid meetingId, Guid creatorId, MeetingRequest request, CancellationToken ct = default);
    }
}
