using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserTimeOffRepository : IRepository<UserTimeOff, Guid>
    {
        Task<List<UserTimeOff>> GetUserTimeOffByDateAsync(Guid userId, DateOnly date, CancellationToken ct = default);
        Task<List<UserTimeOff>> GetUserActiveTimeOffsAsync(Guid userId, CancellationToken ct = default);
    }
}
