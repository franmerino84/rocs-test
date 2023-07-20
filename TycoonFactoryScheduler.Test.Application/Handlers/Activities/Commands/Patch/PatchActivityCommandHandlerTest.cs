using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Abstractions.Persistence.Activities;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;

namespace TycoonFactoryScheduler.Test.Application.Handlers.Activities.Commands.Patch
{
    [TestFixture]
    [Category(Category.Unit)]
    public class PatchActivityCommandHandlerTest
    {        
        private PatchActivityCommandHandler _handler;
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<PatchActivityCommandHandler>> _loggerMock;
        private Mock<Activity> _activityMock;
        private Mock<IActivitiesRepository> _activitiesRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<PatchActivityCommandHandler>>();
            _activityMock = new Mock<Activity>();
            _activitiesRepositoryMock = new Mock<IActivitiesRepository>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Activities).Returns(_activitiesRepositoryMock.Object);

            _handler = new PatchActivityCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Handle_Null_Throws_Null_Exception()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                 _handler.Handle(null, CancellationToken.None));
        }

        [Test]
        public void Handle_Parameter_With_End_Before_Start_Throws_ArgumentException_Containing_ArgumentException_Containing_After()
        {
            var command = new PatchActivityCommand
            {
                Start = new DateTime(2023, 2, 1),
                End = new DateTime(2023, 1, 1),
                Id = 1,
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("after"));
        }

        [Test]
        public void Handle_UnitOfWork_Activities_GetByIdIncludingWorkersIncludingActivities_Throwing_EntityNotFoundInDatabaseException_Throws_It()
        {
            var command = GetValidPatchActivityCommand();
            var exception = new EntityNotFoundInDatabaseException(typeof(Activity), command.Id);

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Throws(exception);

            Assert.ThrowsAsync<EntityNotFoundInDatabaseException>(()=>_handler.Handle(command, default));
        }

        [Test]
        public async Task Handle_UnitOfWork_Activities_GetByIdIncludingWorkersIncludingActivities_Throwing_EntityNotFoundInDatabaseException_LogsDebug_And_LogsError()
        {
            var command = GetValidPatchActivityCommand();
            var exception = new EntityNotFoundInDatabaseException(typeof(Activity), command.Id);

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Throws(exception);

            try
            {
                await _handler.Handle(command, default);
            }
            catch { /**/}

            _loggerMock.VerifyLogContains(LogLevel.Debug, "wasn't found in the database");
            _loggerMock.VerifyLogContains(LogLevel.Error, "wasn't found in the database");
        }

        [Test]
        public void Handle_UnitOfWork_Activities_Delete_Throwing_Exception_Throws_UnexpectedApplicationException()
        {
            var command = GetValidPatchActivityCommand();
            var exception = new Exception();

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Throws(exception);

            Assert.ThrowsAsync<UnexpectedApplicationException>(() => _handler.Handle(command, default));
        }
        
        [Test]
        public async Task Handle_UnitOfWork_Activities_Delete_Throwing_LogsDebug_It()
        {
            var command = GetValidPatchActivityCommand();
            var exception = new Exception();

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Throws(exception);

            try
            {
                await _handler.Handle(command, default);
            }
            catch { /**/ }

            _loggerMock.VerifyLog(LogLevel.Debug, exception.ToString());
        }

        [Test]
        public async Task Handle_UnitOfWork_Activities_Delete_Throwing_LogsError()
        {
            var command = GetValidPatchActivityCommand();
            var exception = new Exception();

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Throws(exception);

            try
            {
                await _handler.Handle(command, default);
            }
            catch { /**/ }

            _loggerMock.VerifyLog(LogLevel.Error);
        }

        [Test]
        public void Handle_Activity_UpdateDates_Throwing_ScheduleConflictException_Throws_It()
        {
            var command = GetValidPatchActivityCommand();
            var activity = _activityMock.Object;
            var exception = new ScheduleConflictException(
                new Worker('A', "a", "a", DateTime.Now),
                activity,
                activity);
            
            activity.Id = 1;
            _activityMock.Setup(x => x.UpdateDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws(exception);

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);

            var resultException = Assert.ThrowsAsync<ScheduleConflictException>(() => _handler.Handle(command, default));

            Assert.That(resultException, Is.EqualTo(exception));
        }

        [Test]
        public async Task Handle_Activity_UpdateDates_Throwing_ScheduleConflictException_LogsError_And_LogsDebug()
        {
            var command = GetValidPatchActivityCommand();
            var activity = _activityMock.Object;
            var exception = new ScheduleConflictException(
                new Worker('A', "a", "a", DateTime.Now),
                activity,
                activity);

            activity.Id = 1;
            _activityMock.Setup(x => x.UpdateDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws(exception);

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);

            try
            {
                await _handler.Handle(command, default);
            }
            catch { /**/}

            _loggerMock.VerifyLogContains(LogLevel.Error, "Cannot assign the activity 1 to the worker A");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "Cannot assign the activity 1 to the worker A");
        }

        [Test]
        public void Handle_Activity_UpdateDates_Throwing_TycoonFactorySchedulerAggregationException_Throws_it()
        {
            var command = GetValidPatchActivityCommand();
            var activity = _activityMock.Object;
            var exception = new TycoonFactorySchedulerAggregationException(
                new ScheduleConflictException(
                    new Worker('A', "a", "a", DateTime.Now),
                    activity,
                    activity),
                new ScheduleConflictException(
                    new Worker('B', "b", "b", DateTime.Now),
                    activity,
                    activity));

            activity.Id = 1;
            _activityMock.Setup(x => x.UpdateDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws(exception);

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);

            var resultException = Assert.ThrowsAsync<TycoonFactorySchedulerAggregationException>(() => _handler.Handle(command, default));

            Assert.That(resultException, Is.EqualTo(exception));
        }

        [Test]
        public async Task Handle_Activity_UpdateDates_Throwing_TycoonFactorySchedulerAggregationException__LogsError_And_LogsDebug()
        {
            var command = GetValidPatchActivityCommand();
            var activity = _activityMock.Object;
            var exception = new TycoonFactorySchedulerAggregationException(
                new ScheduleConflictException(
                    new Worker('A', "a", "a", DateTime.Now),
                    activity,
                    activity),
                new ScheduleConflictException(
                    new Worker('B', "b", "b", DateTime.Now),
                    activity,
                    activity));

            activity.Id = 1;
            _activityMock.Setup(x => x.UpdateDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws(exception);

            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);

            try
            {
                await _handler.Handle(command, default);
            }
            catch { /**/}

            _loggerMock.VerifyLogContains(LogLevel.Error, "Cannot assign the activity 1 to the worker A");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "Cannot assign the activity 1 to the worker A");
            _loggerMock.VerifyLogContains(LogLevel.Error, "Cannot assign the activity 1 to the worker B");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "Cannot assign the activity 1 to the worker B");
        }

        [Test]
        public async Task Handle_Ok_Return_UpdatedObject()
        {
            var command = GetValidPatchActivityCommand();
            var activity = new BuildComponentActivity(1, "description", DateTime.Now, DateTime.Now);
            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);
            var response = GetValidPatchActivityCommandResponse();
            response.Start = command.Start;
            response.End = command.End;
            _mapperMock.Setup(x => x.Map<PatchActivityCommandResponse>(It.IsAny<Activity>())).Returns(response);

            var result = await _handler.Handle(command, default);

            Assert.That(result, Is.EqualTo(response));
        }

        [Test]
        public async Task Handle_Ok_Calls_ActivitiesRepository_Update()
        {
            var command = GetValidPatchActivityCommand();
            var activity = new BuildComponentActivity(1, "description", DateTime.Now, DateTime.Now);
            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);
            var response = GetValidPatchActivityCommandResponse();
            response.Start = command.Start;
            response.End = command.End;
            _mapperMock.Setup(x => x.Map<PatchActivityCommandResponse>(It.IsAny<Activity>())).Returns(response);

            await _handler.Handle(command, default);

            _activitiesRepositoryMock.Verify(x=>x.Update(It.IsAny<Activity>()));
        }

        [Test]
        public async Task Handle_Ok_Calls_UnitOfWork_SaveAsync()
        {
            var command = GetValidPatchActivityCommand();
            var activity = new BuildComponentActivity(1, "description", DateTime.Now, DateTime.Now);
            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);
            var response = GetValidPatchActivityCommandResponse();
            response.Start = command.Start;
            response.End = command.End;
            _mapperMock.Setup(x => x.Map<PatchActivityCommandResponse>(It.IsAny<Activity>())).Returns(response);

            await _handler.Handle(command, default);

            _unitOfWorkMock.Verify(x => x.SaveAsync());
        }

        [Test]
        public async Task Handle_Ok_LogsInformation()
        {
            var command = GetValidPatchActivityCommand();
            var activity = new BuildComponentActivity(1, "description", DateTime.Now, DateTime.Now);
            _activitiesRepositoryMock.Setup(x => x.GetByIdIncludingWorkersIncludingActivities(command.Id)).Returns(activity);
            var response = GetValidPatchActivityCommandResponse();
            response.Start = command.Start;
            response.End = command.End;
            _mapperMock.Setup(x => x.Map<PatchActivityCommandResponse>(It.IsAny<Activity>())).Returns(response);

            await _handler.Handle(command, default);

            _loggerMock.VerifyLogContains(LogLevel.Information, "Updated");

        }

        private static PatchActivityCommandResponse GetValidPatchActivityCommandResponse() =>
            new()
            {
                ActivityType = ActivityType.BuildComponent,
                Description = "description",
                Id = 1,
                Workers = new List<char> { 'a' }
            };

        private static PatchActivityCommand GetValidPatchActivityCommand() =>
            new()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                Id = 1,
            };
    }
}
