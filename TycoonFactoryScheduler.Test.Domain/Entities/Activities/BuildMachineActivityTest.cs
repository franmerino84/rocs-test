using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;

namespace TycoonFactoryScheduler.Test.Domain.Entities.Activities
{
    [TestFixture]
    [Category(Category.Unit)]
    public class BuildMachineActivityTest
    {
        [Test]
        public void BuildMachineActivity_GetChargeBatteriesDuration_Returns_4hours()
        {
            var activity = new BuildMachineActivity("a", DateTime.Now, DateTime.Now);

            Assert.That(activity.GetChargeBatteriesDuration(), Is.EqualTo(TimeSpan.FromHours(4)));
        }

        [Test]
        public void BuildMachineActivity_GetActivityType_Returns_BuildMachine()
        {
            var activity = new BuildMachineActivity("a", DateTime.Now, DateTime.Now);

            Assert.That(activity.GetActivityType(), Is.EqualTo(ActivityType.BuildMachine));
        }

        [Test]
        public void BuildMachineActivity_AddWorkers_Null_Throws_ArgumentNullException()
        {
            var activity = new BuildMachineActivity("a", DateTime.Now, DateTime.Now);

            Assert.Throws<ArgumentNullException>(() => activity.AddWorkers(null));
        }

        [Test]
        public void BuildMachineActivity_AddWorkers_OneWorker_AddsWorker()
        {
            var activity = new BuildMachineActivity("a", DateTime.Now, DateTime.Now);
            var worker = new Worker('a', "a", "a", DateTime.Now);

            activity.AddWorkers(new List<Worker> { worker });

            Assert.That(activity.Workers, Has.Count.EqualTo(1));
            Assert.That(activity.Workers, Does.Contain(worker));
        }

        [Test]
        public void BuildMachineActivity_AddWorkers_OneWorker_WithConflictingActivity_Throws_ScheduleConflictException()
        {
            var activity = new BuildMachineActivity("a", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2));
            var worker = new Worker('a', "a", "a", DateTime.Now,
                new List<Activity> { new BuildMachineActivity("b", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2)) });

            var exception = Assert.Throws<ScheduleConflictException>(() => activity.AddWorkers(new List<Worker> { worker }));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Worker, Is.EqualTo(worker));
                Assert.That(exception.NewActivity, Is.EqualTo(activity));
                Assert.That(exception.ConflictedActivity, Is.EqualTo(worker.Activities.First()));
            });
        }

        [Test]
        public void BuildMachineActivity_AddWorkers_OneWorker_WithTwoConflictingActivity_Throws_TycoonFactorySchedulerAggregationException()
        {
            var activity = new BuildMachineActivity("a", new DateTime(2010, 1, 1), new DateTime(2010, 1, 4));
            var worker = new Worker('a', "a", "a", DateTime.Now,
                new List<Activity> {
                    new BuildMachineActivity("b", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2)),
                    new BuildMachineActivity("b", new DateTime(2010, 1, 3), new DateTime(2010, 1, 4))
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
        public void BuildMachineActivity_UpdateDates_NoConflict_UpdatesThem()
        {
            var activity = new BuildMachineActivity("a", DateTime.Now, DateTime.Now);
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
        public void BuildMachineActivity_UpdateDates_With_One_Conflict_Throws_ScheduleConflictException()
        {
            var activity = new BuildMachineActivity(2, "a", DateTime.Now, DateTime.Now);

            DateTime start = new(2010, 1, 1);
            DateTime end = new(2010, 1, 2);

            var workers = new List<Worker> { new Worker('a', "a", "a", DateTime.Now, new List<Activity>
            {
                new BuildMachineActivity(1,"b",start,end)
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
        public void BuildMachineActivity_UpdateDates_With_Several_Conflicts_Throws_TycoonFactorySchedulerAggregationException()
        {
            var activity = new BuildMachineActivity(2, "a", DateTime.Now, DateTime.Now);

            DateTime start = new(2010, 1, 1);
            DateTime end = new(2010, 1, 4);

            var workers = new List<Worker> { new Worker('a', "a", "a", DateTime.Now, new List<Activity>
            {
                new BuildMachineActivity(1,"b",start,new DateTime(2010, 1, 2)),
                new BuildMachineActivity(3,"c",new DateTime(2010,1,3),end)
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
