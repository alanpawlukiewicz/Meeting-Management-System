using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserRoleRepository : IRepository<UserRole, Guid>
    {
    }
}
