using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers.Models;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories.Workers
{
    public class WorkersRepository : GenericRepository<Worker>, IWorkersRepository
    {
        public WorkersRepository(TycoonFactorySchedulerContext context) : base(context) { }

        public async Task<IEnumerable<BusyWorker>> GetTopBusy(DateTime start, DateTime end, uint size)
        {
            var workersDurationsPair = await _context.Workers
                .Include(x => x.Activities)
                .Select(worker => new
                {
                    Worker = worker,
                    Durations = worker.Activities
                        .Where(activity => activity.Start < end && activity.End > start)
                        .Select(activity => GetTrimmedDuration(activity, start, end))
                })
                .Where(x => x.Durations.Any())
                .ToListAsync();

            // ef core doesn't support aggregations over nested properties yet, so this part has to be treated at client side
            return workersDurationsPair
                .Select(x => new BusyWorker(
                    x.Worker,
                    TimeSpan.FromTicks(x.Durations.Sum(y => y))
                ))
                .OrderByDescending(x => x.TimeBusy)
                .Take((int)size);
        }

        private static long GetTrimmedDuration(Activity activity, DateTime start, DateTime end) =>
            Math.Min(activity.End.Ticks, end.Ticks) - Math.Max(activity.Start.Ticks, start.Ticks);

        public IQueryable<Worker> GetById(IEnumerable<char> ids)
        {
            var workers = _dbSet
                .Where(x => ids.Contains(x.Id));                

            var foundIds = workers.Select(x => x.Id);

            var notFoundWorkers = ids.Where(id => !foundIds.Contains(id)).ToList();

            if (notFoundWorkers.Count > 0)
            {
                if (notFoundWorkers.Count == 1)
                    throw new EntityNotFoundInDatabaseException(typeof(Worker), notFoundWorkers.First());

                throw new TycoonFactorySchedulerAggregationException(notFoundWorkers.Select(x => new EntityNotFoundInDatabaseException(typeof(Worker), x)));
            }

            return workers;
        }

        public IQueryable<Worker> GetByIdIncludingActivities(IEnumerable<char> ids) =>
            GetById(ids).Include(x => x.Activities);

    }
}
