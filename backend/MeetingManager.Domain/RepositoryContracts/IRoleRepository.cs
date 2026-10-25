using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        Task<List<Role>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
        Task<Role?> FindRoleByNameAsync(string name, CancellationToken ct = default);
    }
}
