using TycoonFactoryScheduler.Infrastructure.Persistence.Memory.Repositories;


namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.Memory.Repositories
{
    public class BaseRepositoryTest
    {
        private TestRepository _testRepository;

        [SetUp]
        public void Setup()
        {
            _testRepository = new TestRepository();
        }

        [Test]
        [Category(TestType.Unit)]
        public void GetEntityType_ReturnsParemeterizedType() => 
            Assert.That(_testRepository.GetEntityType(), Is.EqualTo(typeof(BaseRepositoryTest)));


        private class TestRepository : BaseRepository<BaseRepositoryTest>
        {
            public override void Rollback()
            {
                throw new NotImplementedException("Not needed for the test.");
            }

            public override void SaveChanges()
            {
                throw new NotImplementedException("Not needed for the test.");
            }
        }
    }


}
