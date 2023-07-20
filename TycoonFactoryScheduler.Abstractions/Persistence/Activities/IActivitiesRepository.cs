using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Abstractions.Persistence.Activities
{
    public interface IActivitiesRepository : IGenericRepository<Activity>
    {
        Activity GetByIdIncludingWorkers(int id);
        Activity GetByIdIncludingWorkersIncludingActivities(int id);
    }
}
