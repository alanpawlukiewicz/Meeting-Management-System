using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserUpdaterService
    {
        Task GiveUserOrganizerRoleAsync(Guid userId, CancellationToken ct = default);
        Task RevokeUsersOrganizerRoleAsync(Guid userId, CancellationToken ct = default);
    }
}
