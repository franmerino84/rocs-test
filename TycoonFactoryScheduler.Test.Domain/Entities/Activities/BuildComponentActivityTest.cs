using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Activities.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;

namespace TycoonFactoryScheduler.Test.Domain.Entities.Activities
{
    [TestFixture]
    [Category(Category.Unit)]
    public class BuildComponentActivityTest
    {
        [Test]
        public void BuildComponentActivity_GetChargeBatteriesDuration_Returns_2hours()
        {
            var activity = new BuildComponentActivity("a", DateTime.Now, DateTime.Now);

            Assert.That(activity.GetChargeBatteriesDuration(), Is.EqualTo(TimeSpan.FromHours(2)));
        }

        [Test]
        public void BuildComponentActivity_GetActivityType_Returns_BuildComponent()
        {
            var activity = new BuildComponentActivity("a", DateTime.Now, DateTime.Now);

            Assert.That(activity.GetActivityType(), Is.EqualTo(ActivityType.BuildComponent));
        }

        [Test]
        public void BuildComponentActivity_AddWorkers_Null_Throws_ArgumentNullException()
        {
            var activity = new BuildComponentActivity("a", DateTime.Now, DateTime.Now);

            Assert.Throws<ArgumentNullException>(() => activity.AddWorkers(null));
        }

        [Test]
        public void BuildComponentActivity_AddWorkers_OneWorker_AddsWorker()
        {
            var activity = new BuildComponentActivity("a", DateTime.Now, DateTime.Now);
            var worker = new Worker('a', "a", "a", DateTime.Now);

            activity.AddWorkers(new List<Worker> { worker });

            Assert.That(activity.Workers, Has.Count.EqualTo(1));
            Assert.That(activity.Workers, Does.Contain(worker));
        }

        [Test]
        public void BuildComponentActivity_AddWorkers_OneWorker_WithConflictingActivity_Throws_ScheduleConflictException()
        {
            var activity = new BuildComponentActivity("a", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2));
            var worker = new Worker('a', "a", "a", DateTime.Now,
                new List<Activity> { new BuildComponentActivity("b", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2)) });

            var exception = Assert.Throws<ScheduleConflictException>(() => activity.AddWorkers(new List<Worker> { worker }));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Worker, Is.EqualTo(worker));
                Assert.That(exception.NewActivity, Is.EqualTo(activity));
                Assert.That(exception.ConflictedActivity, Is.EqualTo(worker.Activities.First()));
            });
        }

        [Test]
        public void BuildComponentActivity_AddWorkers_OneWorker_WithTwoConflictingActivity_Throws_TycoonFactorySchedulerAggregationException()
        {
            var activity = new BuildComponentActivity("a", new DateTime(2010, 1, 1), new DateTime(2010, 1, 4));
            var worker = new Worker('a', "a", "a", DateTime.Now,
                new List<Activity> {
                    new BuildComponentActivity("b", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2)),
                    new BuildComponentActivity("b", new DateTime(2010, 1, 3), new DateTime(2010, 1, 4))
                });

            var exception = Assert.Throws<TycoonFactorySchedulerAggregationException>(() => activity.AddWorkers(new List<Worker> { worker }));

            Assert.Multiple(() =>
            {
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.First()).Worker, Is.EqualTo(worker));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.First()).NewActivity, Is.EqualTo(activity));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.First()).ConflictedActivity, Is.EqualTo(worker.Activities.First()));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.Skip(1).First()).Worker, Is.EqualTo(worker));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.Skip(1).First()).NewActivity, Is.EqualTo(activity));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.Skip(1).First()).ConflictedActivity, Is.EqualTo(worker.Activities.Skip(1).First()));
            });
        }


        [Test]
        public void BuildComponentActivity_AddWorkers_TwoWorker_Throws_MaximumNumberOfWorkersAllowedExceededException()
        {
            var activity = new BuildComponentActivity("a", DateTime.Now, DateTime.Now);
            var workers = new List<Worker> {
                new Worker('a', "a", "a", DateTime.Now),
                new Worker('b', "b", "b", DateTime.Now)
            };

            Assert.Throws<MaximumNumberOfWorkersAllowedExceededException>(() => activity.AddWorkers(workers));
        }

        [Test]
        public void BuildComponentActivity_UpdateDates_NoConflict_UpdatesThem()
        {
            var activity = new BuildComponentActivity("a", DateTime.Now, DateTime.Now);
            var workers = new List<Worker> { new Worker('a', "a", "a", DateTime.Now) };

            activity.AddWorkers(workers);

            DateTime start = new(2010, 1, 1);
            DateTime end = new(2010, 1, 2);
            activity.UpdateDates(start, end);

            Assert.Multiple(() =>
            {
                Assert.That(activity.Start, Is.EqualTo(start));
                Assert.That(activity.End, Is.EqualTo(end));
            });
        }

        [Test]
        public void BuildComponentActivity_UpdateDates_With_One_Conflict_Throws_ScheduleConflictException()
        {
            var activity = new BuildComponentActivity(2, "a", DateTime.Now, DateTime.Now);

            DateTime start = new(2010, 1, 1);
            DateTime end = new(2010, 1, 2);

            var workers = new List<Worker> { new Worker('a', "a", "a", DateTime.Now, new List<Activity>
            {
                new BuildComponentActivity(1,"b",start,end)
            }) };

            activity.AddWorkers(workers);

            var exception = Assert.Throws<ScheduleConflictException>(() => activity.UpdateDates(start, end));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Worker, Is.EqualTo(workers.First()));
                Assert.That(exception.NewActivity, Is.EqualTo(activity));
                Assert.That(exception.ConflictedActivity, Is.EqualTo(workers.First().Activities.First()));
            });
        }

        [Test]
        public void BuildComponentActivity_UpdateDates_With_Several_Conflicts_Throws_TycoonFactorySchedulerAggregationException()
        {
            var activity = new BuildComponentActivity(2, "a", DateTime.Now, DateTime.Now);

            DateTime start = new(2010, 1, 1);
            DateTime end = new(2010, 1, 4);

            var workers = new List<Worker> { new Worker('a', "a", "a", DateTime.Now, new List<Activity>
            {
                new BuildComponentActivity(1,"b",start,new DateTime(2010, 1, 2)),
                new BuildComponentActivity(3,"c",new DateTime(2010,1,3),end)
            }) };

            activity.AddWorkers(workers);

            var exception = Assert.Throws<TycoonFactorySchedulerAggregationException>(() => activity.UpdateDates(start, end));

            Assert.Multiple(() =>
            {
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.First()).Worker, Is.EqualTo(workers.First()));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.First()).NewActivity, Is.EqualTo(activity));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.First()).ConflictedActivity, Is.EqualTo(workers.First().Activities.First()));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.Skip(1).First()).Worker, Is.EqualTo(workers.First()));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.Skip(1).First()).NewActivity, Is.EqualTo(activity));
                Assert.That(((ScheduleConflictException)exception.InnerExceptions.Skip(1).First()).ConflictedActivity, Is.EqualTo(workers.First().Activities.Skip(1).First()));
            });
        }
    }
}
