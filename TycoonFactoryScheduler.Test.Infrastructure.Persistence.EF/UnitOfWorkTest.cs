using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF;

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.EF
{
    [TestFixture]
    [Category(Category.Integration)]
    [Category(Category.Sql)]
    public class UnitOfWorkTest : DatabaseIntegrationTest
    {
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            base.SetUp();
            InitializeUnitOfWork(false);
        }

        private void InitializeUnitOfWork(bool initializeContext)
        {
            if (initializeContext)
                InitializeContext();

            _unitOfWork = new UnitOfWork(Context);
        }

        [TearDown]
        public void Teardown()
        {
            base.TearDown();
            _unitOfWork.Dispose();
        }

        [Test]
        public void UnitOfWork_InsertWithSave_GetById_Persist()
        {
            var worker = new Worker('Z', "model", "manufacturer", DateTime.Now);
            var activity = new BuildComponentActivity("activity", DateTime.Now, DateTime.Now.AddHours(4), new Worker[] { worker });
            _unitOfWork.Workers.Insert(worker);
            _unitOfWork.Activities.Insert(activity);
            _unitOfWork.Save();
            
            InitializeUnitOfWork(true);

            var retrievedWorker = _unitOfWork.Workers.Get(x=>x.Id == worker.Id, includeProperties: nameof(Worker.Activities)).First();
            var retrievedActivity = _unitOfWork.Activities.Get(x=>x.Id == activity.Id, includeProperties: nameof(Activity.Workers)).First();

            Assert.Multiple(() =>
            {
                worker.AssertJsonEqualsTo(retrievedWorker);
                activity.AssertJsonEqualsTo(retrievedActivity);
            });
        }

        [Test]
        public void UnitOfWork_InsertWithoutSave_GetById_ThrowsEntityNotFoundInDatabaseException()
        {
            var worker = new Worker('Z', "model", "manufacturer", DateTime.Now);
            _unitOfWork.Workers.Insert(worker);

            InitializeUnitOfWork(true);

            Assert.Throws<EntityNotFoundInDatabaseException>(() => _unitOfWork.Workers.GetById(worker.Id));
        }
    }
}
