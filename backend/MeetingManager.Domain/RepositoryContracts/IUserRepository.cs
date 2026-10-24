using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> FindUserAsync(Guid userId, CancellationToken ct = default);
        Task<User?> FindUserByLoginAsync(string login, CancellationToken ct = default);

        Task<User?> FindUserWithRolesAsync(Guid userId, CancellationToken ct = default);

        Task<List<Guid>> GetAllUserIdsAsync(CancellationToken ct = default);
        Task<List<User>> GetUsersWithAvailabilitiesAsync(List<Guid>? userIds, DateOnly date, CancellationToken ct = default);

        Task<List<User>> GetUsersWithLimitAsync(string query, int limit = 10, CancellationToken ct = default);
    }
}
