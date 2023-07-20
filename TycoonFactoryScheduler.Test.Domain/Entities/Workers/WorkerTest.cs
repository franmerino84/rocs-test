using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;

namespace TycoonFactoryScheduler.Test.Domain.Entities.Workers
{
    [TestFixture]
    [Category(Category.Unit)]
    public class WorkerTest
    {
        [Test]
        public void Worker_AddActivity_WithoutActivities_AddsActivity()
        {
            var worker = new Worker('a', "a1", "a2", DateTime.Now, new List<Activity>());
            var activity = new BuildComponentActivity("b1", DateTime.Now, DateTime.Now);
            
            worker.AddActivity(activity);

            Assert.That(worker.Activities, Does.Contain(activity));
        }

        [Test]
        public void Worker_AddActivity_WithoutConflictedActivities_AddsActivity()
        {
            var worker = new Worker('a', "a1", "a2", DateTime.Now, 
                new List<Activity> { new BuildComponentActivity(1, "c1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2))});

            var activity = new BuildComponentActivity("b1", DateTime.Now, DateTime.Now);

            worker.AddActivity(activity);

            Assert.That(worker.Activities, Has.Count.EqualTo(2));
            Assert.That(worker.Activities, Does.Contain(activity));
        }

        [Test]
        public void Worker_AddActivity_With_One_ConflictedActivity_Throws_ScheduledConflictActivity()
        {
            var worker = new Worker('a', "a1", "a2", DateTime.Now,
                new List<Activity> { new BuildComponentActivity(1, "c1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2)) });

            var activity = new BuildComponentActivity("b1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2));

            var exception = Assert.Throws<ScheduleConflictException>(()=>worker.AddActivity(activity));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Worker, Is.EqualTo(worker));
                Assert.That(exception.NewActivity, Is.EqualTo(activity));
                Assert.That(exception.ConflictedActivity, Is.EqualTo(worker.Activities.First()));
            });
        }

        [Test]
        public void Worker_AddActivity_With_Several_ConflictedActivities_Throws_TycoonFactorySchedulerAggregationException()
        {
            var worker = new Worker('a', "a1", "a2", DateTime.Now,
                new List<Activity> { 
                    new BuildComponentActivity(1, "c1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 2)),
                    new BuildComponentActivity(2, "d1", new DateTime(2010, 1, 3), new DateTime(2010, 1, 4))
                });

            var activity = new BuildComponentActivity("b1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 4));

            var exception = Assert.Throws<TycoonFactorySchedulerAggregationException>(() => worker.AddActivity(activity));

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
    }
}
