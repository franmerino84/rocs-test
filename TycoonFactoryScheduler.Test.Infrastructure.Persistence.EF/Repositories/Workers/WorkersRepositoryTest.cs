using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds;

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.EF.Repositories.Workers
{
    [Category(Category.Integration)]
    [Category(Category.Sql)]
    public class WorkersRepositoryTest : DatabaseIntegrationTest
    {
        private WorkersRepository _repository;

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

            _repository = new WorkersRepository(Context);
        }

        [Test]
        public void GetByIdIncludingActivities_Inexistent_Id_Throws_EntityNotFoundInDatabaseException()
        {
            var exception = Assert.Throws<EntityNotFoundInDatabaseException>(()=> _repository.GetByIdIncludingActivities(new List<char> { 'z' }));

            Assert.Multiple(() =>
            {
                Assert.That(exception.EntityType, Is.EqualTo(typeof(Worker)));
                Assert.That(exception.EntityId, Is.EqualTo('z'));
            });
        }

        [Test]
        public void GetByIdIncludingActivities_Inexistent_Id_Throws_TycoonFactorySchedulerAggregationException()
        {
            var exception = Assert.Throws<TycoonFactorySchedulerAggregationException>(() => _repository.GetByIdIncludingActivities(new List<char> { 'z','y' }));            
            Assert.Multiple(() =>
            {
                Assert.That(((EntityNotFoundInDatabaseException)exception.InnerExceptions.First()).EntityType, Is.EqualTo(typeof(Worker)));
                Assert.That(((EntityNotFoundInDatabaseException)exception.InnerExceptions.First()).EntityId, Is.EqualTo('z'));
                Assert.That(((EntityNotFoundInDatabaseException)exception.InnerExceptions.Skip(1).First()).EntityType, Is.EqualTo(typeof(Worker)));
                Assert.That(((EntityNotFoundInDatabaseException)exception.InnerExceptions.Skip(1).First()).EntityId, Is.EqualTo('y'));
            });
        }

        [Test]
        public void GetByIdIncludingActivities_Existent_Ids_Returns_Workers_Including_Activities()
        {
            var result = _repository.GetByIdIncludingActivities(new List<char> { 'A','B' }).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result[0].Model, Is.EqualTo(WorkerSeeds.WallE.Model));
                Assert.That(result[0].Activities, Has.Count.EqualTo(3));
                Assert.That(result[1].Model, Is.EqualTo(WorkerSeeds.NS5.Model));
                Assert.That(result[1].Activities, Has.Count.EqualTo(3));
            });
        }

        [Test]
        public async Task GetTopBusy_Returns_TopBusy()
        {
            var result = (await _repository.GetTopBusy(new DateTime(2023, 3, 3), new DateTime(2023, 3, 10), 10)).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(10));
                Assert.That(result[0].Worker.Id, Is.EqualTo('D'));
                Assert.That(result[0].TimeBusy, Is.EqualTo(new TimeSpan(1,12,0,0)));
                Assert.That(result[1].Worker.Id, Is.EqualTo('A'));
                Assert.That(result[1].TimeBusy, Is.EqualTo(new TimeSpan(1, 6, 0, 0)));
            });
        }
    }
}
