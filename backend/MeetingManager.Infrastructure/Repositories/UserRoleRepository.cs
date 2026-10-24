using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;

namespace MeetingManager.Infrastructure.Repositories
{
    public class UserRoleRepository : Repository<UserRole, Guid>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
