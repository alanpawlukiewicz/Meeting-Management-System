using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthResponse> GenerateJwtToken(User user, CancellationToken ct = default);
    }
}
