using Moq;
using TycoonFactoryScheduler.Domain.Contracts.Persistence.Repositories;
using TycoonFactoryScheduler.Infrastructure.Persistence.Memory;

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.Memory
{
    public class UnitOfWorkTest
    {
        private readonly Mock<IRepository<UnitOfWork>> _repositoryA;
        private readonly Mock<IRepository<UnitOfWorkTest>> _repositoryB;
        private readonly Mock<IRepository<UnitOfWorkTest>> _repositoryC;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTest()
        {
            _repositoryA = new Mock<IRepository<UnitOfWork>>();
            _repositoryB = new Mock<IRepository<UnitOfWorkTest>>();
            _repositoryC = new Mock<IRepository<UnitOfWorkTest>>();
            _unitOfWork = new UnitOfWork(new IRepository[] { _repositoryA.Object, _repositoryB.Object, _repositoryC.Object });
        }

        [Test]
        [Category(TestType.Unit)]

        public void SaveChanges_CallsAllRepositoriesSaveChanges()
        {
            _unitOfWork.SaveChanges();
            _repositoryA.Verify(x => x.SaveChanges());
            _repositoryB.Verify(x => x.SaveChanges());
            _repositoryC.Verify(x => x.SaveChanges());
        }

        [Test]
        [Category(TestType.Unit)]

        public void Rollback_CallsAllRepositoriesRollback()
        {
            _unitOfWork.Rollback();
            _repositoryA.Verify(x => x.Rollback());
            _repositoryB.Verify(x => x.Rollback());
            _repositoryC.Verify(x => x.Rollback());
        }

        [Test]
        [Category(TestType.Unit)]
        public void GetRepository_BringsRepositoryOfSpecifiedType()
        {
            _repositoryA.Setup(x => x.GetEntityType()).Returns(typeof(UnitOfWork));
            _repositoryB.Setup(x => x.GetEntityType()).Returns(typeof(UnitOfWorkTest));
            _repositoryC.Setup(x => x.GetEntityType()).Returns(typeof(UnitOfWorkTest));

            var repository = _unitOfWork.GetRepository<UnitOfWork>();

            Assert.That(repository, Is.EqualTo(_repositoryA.Object));
        }
    }
}
