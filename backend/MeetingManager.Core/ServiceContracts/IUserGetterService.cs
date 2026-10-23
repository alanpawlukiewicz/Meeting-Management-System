using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IUserGetterService
    {
        Task<UserProfile> GetUserProfileAsync(Guid userId, CancellationToken ct = default);

        Task<List<UserStatusCheckResponse>> CheckUserAvailabilitiesAsync(UserStatusCheckRequest request, CancellationToken ct = default);
    }
}
