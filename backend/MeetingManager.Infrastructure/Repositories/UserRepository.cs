using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> FindUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<User?> FindUserByLoginAsync(string login, CancellationToken ct = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.UserName == login, ct);
        }
    }
}
