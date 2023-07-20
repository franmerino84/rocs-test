using TycoonFactoryScheduler.Domain.Contracts.Persistence;
using TycoonFactoryScheduler.Domain.Contracts.Persistence.Repositories;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.Memory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly List<IRepository> _repositories;

        public UnitOfWork(IEnumerable<IRepository> repositories) => 
            _repositories = repositories.ToList();

        public IRepository<T>? GetRepository<T>() => 
            _repositories.FirstOrDefault(repository => 
                typeof(T).Equals(repository.GetEntityType())) as IRepository<T>;

        public void Rollback() =>
            _repositories.ForEach(repository => 
                repository.Rollback());

        public void SaveChanges() => 
            _repositories.ForEach(repository => 
                repository.SaveChanges());
    }
}
