using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserTimeOffDeleterService
    {
        Task DeleteTimeOffAsync(Guid timeOffId, CancellationToken ct = default);
    }
}
