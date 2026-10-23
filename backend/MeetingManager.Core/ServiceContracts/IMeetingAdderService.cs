using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IMeetingAdderService
    {
        Task<MeetingResponse> AddMeetingAsync(Guid creatorId, MeetingRequest request, CancellationToken ct = default);
    }
}
