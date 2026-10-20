using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest request, string? ipAddress, string? userAgent, CancellationToken ct = default);
    }
}
