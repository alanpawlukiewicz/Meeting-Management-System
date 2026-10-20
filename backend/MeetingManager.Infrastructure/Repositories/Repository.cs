using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity) => DbSet.Add(entity);

        public void Update(TEntity entity) => DbSet.Update(entity);

        public void Remove(TEntity entity) => DbSet.Remove(entity);

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => Context.SaveChangesAsync(ct);
    }
}
