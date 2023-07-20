using Microsoft.Extensions.Logging;
using Moq;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Abstractions.Persistence.Activities;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Delete;
namespace TycoonFactoryScheduler.Test.Application.Handlers.Activities.Commands.Delete
{
    [TestFixture]
    [Category(Category.Unit)]
    public class DeleteActivityCommandHandlerTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IActivitiesRepository> _activitiesRepositoryMock;
        private Mock<ILogger<DeleteActivityCommandHandler>> _loggerMock;
        private DeleteActivityCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _activitiesRepositoryMock = new Mock<IActivitiesRepository>();
            _unitOfWorkMock.Setup(x => x.Activities).Returns(_activitiesRepositoryMock.Object);
            _loggerMock = new Mock<ILogger<DeleteActivityCommandHandler>>();
            _handler = new DeleteActivityCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Handle_With_Request_Null_Throws_ArgumentNullException() => 
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, default));

        [Test]
        public async Task Handle_Ok_Success()
        {
            await _handler.Handle(GetValidRequest(), default);

            Assert.Pass();
        }

        [Test]
        public async Task Handle_Ok_LogsInformation()
        {
            await _handler.Handle(GetValidRequest(), default);

            _loggerMock.VerifyLogContains(LogLevel.Information, "deleted");
        }

        [Test]
        public void Handle_UnitOfWork_Activities_Delete_Throwing_Throws_UnexpectedApplicationException()
        {            
            _activitiesRepositoryMock.Setup(x => x.Delete(It.IsAny<object>())).Throws<Exception>();

            Assert.ThrowsAsync<UnexpectedApplicationException>(()=>_handler.Handle(GetValidRequest(), default));
        }

        [Test]
        public async Task Handle_UnitOfWork_Activities_Delete_Throwing_LogsDebug_It()
        {
            var exception = new Exception();

            _activitiesRepositoryMock.Setup(x => x.Delete(It.IsAny<object>())).Throws(exception);

            try
            {
                await _handler.Handle(GetValidRequest(), default);
            }
            catch { /**/ }

            _loggerMock.VerifyLog(LogLevel.Debug, exception.ToString());
        }

        [Test]
        public async Task Handle_UnitOfWork_Activities_Delete_Throwing_LogsError()
        {
            var exception = new Exception();

            _activitiesRepositoryMock.Setup(x => x.Delete(It.IsAny<object>())).Throws(exception);

            try
            {
                await _handler.Handle(GetValidRequest(), default);
            }
            catch { /**/ }

            _loggerMock.VerifyLog(LogLevel.Error);
        }



        private static DeleteActivityCommand GetValidRequest() =>
            new(1);
    }
}
