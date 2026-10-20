using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IRefreshTokenRotationService
    {
        Task RotateAsync(RefreshToken oldToken, RefreshTokenAddRequest newToken, CancellationToken ct = default);
    }
}
