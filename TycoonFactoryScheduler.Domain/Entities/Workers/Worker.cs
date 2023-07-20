using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;

namespace TycoonFactoryScheduler.Domain.Entities.Workers
{
    public class Worker
    {
        public Worker(char id, string model, string manufacturer, DateTime creationDate)
            : this(id, model, manufacturer, creationDate, new List<Activity>())
        {
        }

        public Worker(char id, string model, string manufacturer, DateTime creationDate, ICollection<Activity> activities)
        {
            Id = id;
            Model = model;
            Manufacturer = manufacturer;
            CreationDate = creationDate;
            Activities = activities;
        }

        public char Id { get; set; }

        [MaxLength(32)]
        public string Model { get; set; }

        [MaxLength(32)]
        public string Manufacturer { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public void AddActivity(Activity activity, bool notifyActivity = false)
        {
            ValidateAddActivityParameters(activity);
            
            var conflictedActivities = Activities.Where(activity.IsInConflictWith);

            ManageConflictedActivitiesException(activity, conflictedActivities);

            Activities.Add(activity);

            if (notifyActivity)
                activity.AddWorker(this, notifyWorker: false, checkNumberOfWorkers: true);
        }

        private static void ValidateAddActivityParameters(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));
        }

        internal void HandleActivityDatesUpdate(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            var conflictedActivities = Activities
                .Where(candidateConflictedActivity => candidateConflictedActivity.Id != activity.Id
                    && candidateConflictedActivity.IsInConflictWith(activity));

            ManageConflictedActivitiesException(activity, conflictedActivities);
        }

        private void ManageConflictedActivitiesException(Activity activity, IEnumerable<Activity> conflictedActivities)
        {
            if (!conflictedActivities.Any())
                return;

            if (conflictedActivities.Count() == 1)
                throw new ScheduleConflictException(this, activity, conflictedActivities.First());

            throw new TycoonFactorySchedulerAggregationException(conflictedActivities
                .Select(conflictedActivity => new ScheduleConflictException(this, activity, conflictedActivity)));
        }
    }
}
