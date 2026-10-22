using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IMeetingGetterService
    {
        Task<List<MeetingDto>> GetUserMeetingsAsync(Guid userId, CancellationToken ct = default);
    }
}
