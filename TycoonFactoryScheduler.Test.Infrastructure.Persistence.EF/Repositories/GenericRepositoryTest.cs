using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds;

#pragma warning disable CS8604 // Possible null reference argument.

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.EF.Repositories
{
    [TestFixture]
    [Category(Category.Integration)]
    [Category(Category.Sql)]
    public class GenericRepositoryTest : DatabaseIntegrationTest
    {
        private GenericRepository<Worker> _repository;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            InitializeRepository(false);
        }

        private void InitializeRepository(bool initializeContext)
        {
            if (initializeContext)
                InitializeContext();

            _repository = new GenericRepository<Worker>(Context);
        }

        [TearDown]
        protected override void TearDown() =>
            base.TearDown();

        [Test]
        public void Delete_WorkerFId_Then_SaveChanges_Then_GetById_WorkerF_ThrowsEntityNotFoundInDatabaseException()
        {
            _repository.Delete(WorkerSeeds.Leoben.Id);
            Context.SaveChanges();
            Assert.Throws<EntityNotFoundInDatabaseException>(() => _repository.GetById(WorkerSeeds.Leoben.Id));
        }

        [Test]
        public void GetById_WorkerB_Then_DeleteIt_Then_SaveChanges_Then_TryGetById_WorkerB_ReturnsFalse()
        {
            var ns5 = _repository.GetById(WorkerSeeds.NS5.Id);
            _repository.Delete(ns5);
            Context.SaveChanges();
            var result = _repository.TryGetById(WorkerSeeds.NS5.Id, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetById_WorkerM_ReturnsTrue_And_OutputParameterWithRetrievedEntity()
        {
            var result = _repository.TryGetById(WorkerSeeds.Dolores.Id, out var dolores);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                dolores.AssertJsonEqualsTo(WorkerSeeds.Dolores);
            });
        }

        [Test]
        public void GetById_WorkerR_Then_Update_Then_SaveChanges_Then_GetById_WorkerR_Persist()
        {
            var expectedNexus = _repository.GetById(WorkerSeeds.Nexus.Id);

            expectedNexus.Model = "Nexus 2";
            _repository.Update(expectedNexus);

            Context.SaveChanges();

            InitializeRepository(true);

            var retrievedNexus = _repository.GetById(WorkerSeeds.Nexus.Id);

            retrievedNexus.AssertJsonEqualsTo(expectedNexus);
        }

        [Test]
        public void Get_FilteringByModelName_RetrievesEntity()
        {
            var caprica = _repository.Get(x => x.Model == WorkerSeeds.Caprica.Model).First();

            caprica.AssertJsonEqualsTo(WorkerSeeds.Caprica);
        }
    }
}

#pragma warning restore CS8604 // Possible null reference argument.