using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IRefreshTokenDeleterService
    {
        Task DeleteTokenAsync(string token, CancellationToken ct = default);
    }
}
