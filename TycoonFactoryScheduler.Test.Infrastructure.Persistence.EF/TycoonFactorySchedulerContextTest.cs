using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds.Activities;

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.EF
{
    [TestFixture]
    [Category(Category.Integration)]
    [Category(Category.Sql)]
    public class TycoonFactorySchedulerContextTest : DatabaseIntegrationTest
    {
        [SetUp]
        protected override void SetUp() => 
            base.SetUp();

        [TearDown]
        protected override void TearDown() => 
            base.TearDown();

        [Test]        
        public void SeedsAdds19Workers()
        {
            Assert.That(Context.Workers.Count(), Is.EqualTo(19));
        }

        [Test]
        public void SeedsAdds16Activities()
        {
            Assert.That(Context.Activities.Count(), Is.EqualTo(16));
        }

        [Test]
        public void WorkerAIsRecoveredSameAsSeeded()
        {
            Worker wallE = Context.Workers.First(worker => worker.Id == 'A');

            wallE.AssertJsonEqualsTo(WorkerSeeds.WallE);
        }

        [Test]
        public void BuildComponentActivity1IsRecoveredSameAsSeeded()
        {
            Activity fluxCapacitor = Context.Activities.First(activity => activity.Id == 1);

            fluxCapacitor.AssertJsonEqualsTo(BuildComponentActivitySeeds.FluxCapacitor);
        }

        [Test]
        public void BuildMachineActivity8IsRecoveredSameAsSeeded()
        {
            Activity autonomousCar = Context.Activities.First(activity => activity.Id == 8);

            autonomousCar.AssertJsonEqualsTo(BuildMachineActivitySeeds.AutonomousCar);
        }

    }
}
