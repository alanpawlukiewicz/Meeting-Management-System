using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        Task<List<Role>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
    }
}
