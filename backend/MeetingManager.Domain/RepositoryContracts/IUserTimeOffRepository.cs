using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IUserTimeOffRepository : IRepository<UserTimeOff, Guid>
    {
        Task<List<UserTimeOff>> GetUserTimeOffByDateAsync(Guid userId, DateOnly date, CancellationToken ct = default);
        Task<List<UserTimeOff>> GetUserTimeOffsAsync(Guid userId, CancellationToken ct = default);
        Task<UserTimeOff?> FindUserTimeOffAsync(Guid timeOffId, CancellationToken ct = default);
    }
}
