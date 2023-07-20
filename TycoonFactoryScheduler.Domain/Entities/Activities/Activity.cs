using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. - false positive

namespace TycoonFactoryScheduler.Domain.Entities.Activities
{
    public abstract class Activity
    {
        [Obsolete(message: "Don't use this constructor. It's here only for Mocking and Mapping purposes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed", Justification = "False positive")]
        protected Activity() { }

        protected Activity(int id, string description, DateTime start, DateTime end)
            : this(id, description, start, end, new List<Worker>()) { }

        protected Activity(int id, string description, DateTime start, DateTime end, ICollection<Worker> workers)
        {
            Id = id;
            Description = description;
            Start = start;
            End = end;
            Workers = workers;
        }


        public abstract ActivityType GetActivityType();
        public abstract TimeSpan GetChargeBatteriesDuration();

        internal void AddWorker(Worker worker, bool notifyWorker = true, bool checkNumberOfWorkers = false)
        {
            ValidateAddWorkerParameters(worker);

            Workers.Add(worker);

            if (checkNumberOfWorkers)
                ManageNumberWorkersLimits();

            if (notifyWorker)
                worker.AddActivity(this, notifyActivity: false);
        }

        private static void ValidateAddWorkerParameters(Worker worker)
        {
            if (worker == null)
                throw new ArgumentNullException(nameof(worker));
        }

        private void ManageNumberWorkersLimits()
        {
            if (Workers.Count < MinimumNumberOfWorkersNeeded)
                throw new MinimumNumberOfWorkersNeededNotMetException(this);

            if (Workers.Count > MaximumNumberOfWorkersAllowed)
                throw new MaximumNumberOfWorkersAllowedExceededException(this);
        }

        public virtual void AddWorkers(IEnumerable<Worker> workers, bool notifyWorkers = true)
        {
            ValidateAddWorkersParameters(workers);

            var conflicts = new List<ScheduleConflictException>();
            foreach (var worker in workers)
            {
                try
                {
                    AddWorker(worker, notifyWorkers);
                }
                catch (ScheduleConflictException ex)
                {
                    conflicts.Add(ex);
                }
                catch (TycoonFactorySchedulerAggregationException ex)
                {
                    conflicts.AddRange(ex.InnerExceptions.Select(x => (ScheduleConflictException)x));
                }
            }

            if (conflicts.Count > 0)
            {
                if (conflicts.Count == 1)
                    throw conflicts.First();

                throw new TycoonFactorySchedulerAggregationException(conflicts);
            }

            ManageNumberWorkersLimits();
        }

        private static void ValidateAddWorkersParameters(IEnumerable<Worker> workers)
        {
            if (workers == null)
                throw new ArgumentNullException(nameof(workers));
        }

        internal bool IsInConflictWith(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            return EndConsideringRecharge > activity.Start && Start < activity.EndConsideringRecharge;
        }

        public virtual void UpdateDates(DateTime start, DateTime end)
        {
            Start = start;
            End = end;

            var conflicts = new List<ScheduleConflictException>();
            foreach (var worker in Workers)
            {
                try
                {
                    worker.HandleActivityDatesUpdate(this);
                }
                catch (ScheduleConflictException ex)
                {
                    conflicts.Add(ex);
                }
                catch (TycoonFactorySchedulerAggregationException ex)
                {
                    conflicts.AddRange(ex.InnerExceptions.Select(x => (ScheduleConflictException)x));
                }
            }

            if (conflicts.Count > 0)
            {
                if (conflicts.Count == 1)
                    throw conflicts.First();

                throw new TycoonFactorySchedulerAggregationException(conflicts);
            }
        }

        [MaxLength(100)]
        public string Description { get; set; }

        public TimeSpan Duration =>
            End - Start;

        public TimeSpan DurationConsideringRecharge { get { return Duration + GetChargeBatteriesDuration(); } }

        public DateTime End { get; set; }

        public DateTime EndConsideringRecharge =>
            End + GetChargeBatteriesDuration();

        public int Id { get; set; }

        public DateTime Start { get; set; }

        public ICollection<Worker> Workers { get; set; }
        public abstract int MinimumNumberOfWorkersNeeded { get; }
        public abstract int MaximumNumberOfWorkersAllowed { get; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. - false positive
