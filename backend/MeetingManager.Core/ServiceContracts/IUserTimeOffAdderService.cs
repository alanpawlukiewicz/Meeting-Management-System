using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserTimeOffAdderService
    {
        Task<UserTimeOffResponse> AddUserTimeOffAsync(Guid userId, UserTimeOffAddRequest request, CancellationToken ct = default);
    }
}
