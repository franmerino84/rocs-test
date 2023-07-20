using TycoonFactoryScheduler.Domain.Contracts.Persistence.Repositories;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.Memory.Repositories
{
    public abstract class BaseRepository<T> : IRepository
    {
        public Type GetEntityType() => 
            typeof(T);

        public abstract void Rollback();

        public abstract void SaveChanges();
    }
}
