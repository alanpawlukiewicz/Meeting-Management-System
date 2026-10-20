using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> FindUserAsync(Guid userId, CancellationToken ct = default);
        Task<User?> FindUserByLoginAsync(string login, CancellationToken ct = default);
    }
}
