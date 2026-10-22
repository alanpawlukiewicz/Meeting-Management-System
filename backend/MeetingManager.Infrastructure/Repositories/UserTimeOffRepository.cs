using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Repositories
{
    public class UserTimeOffRepository : Repository<UserTimeOff, Guid>, IUserTimeOffRepository
    {
        public UserTimeOffRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserTimeOff>> GetUserTimeOffByDateAsync(Guid userId, DateOnly date, CancellationToken ct = default)
        {
            return await DbSet.Where(uto => uto.UserId == userId && uto.Date == date).ToListAsync(ct);
        }

        public async Task<List<UserTimeOff>> GetUserActiveTimeOffsAsync(Guid userId, CancellationToken ct = default)
        {
            return await DbSet.AsNoTracking().Where(uto => uto.UserId == userId && uto.Date >= DateOnly.FromDateTime(DateTime.Now)).ToListAsync(ct);
        }
    }
}
