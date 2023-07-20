using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories.Activities;

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.EF.Repositories.Activities
{
    [Category(Category.Integration)]
    [Category(Category.Sql)]
    public class ActivitiesRepositoryTest : DatabaseIntegrationTest
    {
        private ActivitiesRepository _repository;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            InitializeRepository(false);
        }

        [TearDown]
        protected override void TearDown() =>
            base.TearDown();

        private void InitializeRepository(bool initializeContext)
        {
            if (initializeContext)
                InitializeContext();

            _repository = new ActivitiesRepository(Context);
        }

        [Test]
        public void GetByIdIncludingWorkers_Inexistent_Id_Throws_EntityNotFoundInDatabaseException()
        {
            var exception = Assert.Throws<EntityNotFoundInDatabaseException>(()=> _repository.GetByIdIncludingWorkers(100));
            
            Assert.Multiple(() =>
            {
                Assert.That(exception.EntityType, Is.EqualTo(typeof(Activity)));
                Assert.That(exception.EntityId, Is.EqualTo(100));
            });
        }

        [Test]
        public void GetByIdIncludingWorkers_Brings_RelatedWorkers()
        {
            var activity = _repository.GetByIdIncludingWorkers(1);

            Assert.That(activity.Workers, Has.Count.EqualTo(1));
        }

        [Test]
        public void GetByIdIncludingWorkersIncludingActivities_Brings_RelatedWorkers_With_RelatedActivities()
        {
            var activity = _repository.GetByIdIncludingWorkersIncludingActivities(1);

            Assert.That(activity.Workers.First().Activities, Has.Count.EqualTo(3));
        }

        [Test]
        public void GetByIdIncludingWorkersIncludingActivities_Inexistent_Id_Throws_EntityNotFoundInDatabaseException()
        {
            var exception = Assert.Throws<EntityNotFoundInDatabaseException>(() => _repository.GetByIdIncludingWorkersIncludingActivities(100));
            
            Assert.Multiple(() =>
            {
                Assert.That(exception.EntityType, Is.EqualTo(typeof(Activity)));
                Assert.That(exception.EntityId, Is.EqualTo(100));
            });
        }
    }
}
