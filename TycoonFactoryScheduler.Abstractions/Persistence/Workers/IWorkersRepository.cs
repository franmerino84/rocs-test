using TycoonFactoryScheduler.Abstractions.Persistence.Workers.Models;
using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Abstractions.Persistence.Workers
{
    public interface IWorkersRepository : IGenericRepository<Worker>
    {
        Task<IEnumerable<BusyWorker>> GetTopBusy(DateTime start, DateTime end, uint size);
        IQueryable<Worker> GetById(IEnumerable<char> ids);
        IQueryable<Worker> GetByIdIncludingActivities(IEnumerable<char> ids);
    }
}
