using TycoonFactoryScheduler.Abstractions.Persistence.Activities;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers;

namespace TycoonFactoryScheduler.Abstractions.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IActivitiesRepository Activities { get; }
        IWorkersRepository Workers { get; }

        void Save();
        Task SaveAsync();
    }
}