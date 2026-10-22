using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserAvailabilityRepository : IRepository<UserAvailability, Guid>
    {
        Task SetCurrentUserAvailabilitiesForDeleteAsync(Guid userId, CancellationToken ct = default);
        Task<List<UserAvailability>> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default);
    }
}
