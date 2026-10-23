using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IMeetingAttendanceUpdaterService
    {
        Task ChangeAttendanceAsync(Guid meetingId, Guid userId, StatusDto request, CancellationToken ct = default);
    }
}
