using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserTimeOffRepository : IRepository<UserTimeOff, Guid>
    {
        Task<List<UserTimeOff>> FindUserTimeOffByDateAsync(Guid userId, DateOnly date, CancellationToken ct = default);
    }
}
