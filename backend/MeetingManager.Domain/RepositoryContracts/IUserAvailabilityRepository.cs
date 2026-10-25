using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserAvailabilityRepository : IRepository<UserAvailability, Guid>
    {
        Task SetCurrentUserAvailabilitiesForDeleteAsync(Guid userId, CancellationToken ct = default);
        Task<List<UserAvailability>> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default);
    }
}
