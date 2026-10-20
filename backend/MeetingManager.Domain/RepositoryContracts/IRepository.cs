namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
