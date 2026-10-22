using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IRoleAuthService
    {
        Task<bool> HasRoleAsync(Guid userId, string roleName, CancellationToken ct = default);
    }
}
