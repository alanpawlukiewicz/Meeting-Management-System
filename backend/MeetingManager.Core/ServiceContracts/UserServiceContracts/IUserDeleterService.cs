using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts.UserServiceContracts
{
    public interface IUserDeleterService
    {
        Task DeleteUserAsync(Guid userId, CancellationToken ct = default);
    }
}
