using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Abstractions.Persistence.Activities;
using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories.Activities
{
    public class ActivitiesRepository : GenericRepository<Activity>, IActivitiesRepository
    {
        public ActivitiesRepository(TycoonFactorySchedulerContext context) : base(context) { }

        public Activity GetByIdIncludingWorkers(int id) =>
            _dbSet.Include(activity => activity.Workers)
                .FirstOrDefault(activity => activity.Id == id) 
                ?? throw new EntityNotFoundInDatabaseException(typeof(Activity), id);

        public Activity GetByIdIncludingWorkersIncludingActivities(int id) =>
            _dbSet.Include(activity => activity.Workers)
                .ThenInclude(worker=>worker.Activities)
                .FirstOrDefault(activity => activity.Id == id)
                ?? throw new EntityNotFoundInDatabaseException(typeof(Activity), id);
    }
}
