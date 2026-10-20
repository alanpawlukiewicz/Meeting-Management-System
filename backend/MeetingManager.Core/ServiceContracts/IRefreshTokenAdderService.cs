using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IRefreshTokenAdderService
    {
        Task AddTokenAsync(RefreshTokenAddRequest request, CancellationToken ct = default);
    }
}
