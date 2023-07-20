using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Abstractions.Persistence.Activities;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers;
using System.Runtime.InteropServices;

namespace TycoonFactoryScheduler.Test.Application.Handlers.Activities.Commands.Post
{
    [TestFixture]
    [Category(Category.Unit)]
    public class PostActivityCommandHandlerTest
    {
        //TODO
        private PostActivityCommandHandler _handler;
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<PostActivityCommandHandler>> _loggerMock;
        private Mock<Activity> _activityMock;
        private Mock<IActivitiesRepository> _activitiesRepositoryMock;
        private Mock<IWorkersRepository> _workersRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<PostActivityCommandHandler>>();
            _activityMock = new Mock<Activity>();
            _activitiesRepositoryMock = new Mock<IActivitiesRepository>();
            _workersRepositoryMock = new Mock<IWorkersRepository>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Activities).Returns(_activitiesRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);

            _handler = new PostActivityCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
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
            var command = new PostActivityCommand
            {
                Start = new DateTime(2023, 2, 1),
                End = new DateTime(2023, 1, 1),
                ActivityType = ActivityType.BuildComponent,
                Description = "description",
                Workers = new List<char> { 'a' }
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("after"));
        }

        [Test]
        public void Handle_Parameter_With_Description_Length_LowerThan_1_Throws_ArgumentException_Containing_ArgumentException_Containing_Length()
        {
            var command = new PostActivityCommand
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 2, 1),
                ActivityType = ActivityType.BuildComponent,
                Workers = new List<char> { 'a' }
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("length"));
        }

        [Test]
        public void Handle_Parameter_With_Description_Length_GreaterThan_100_Throws_ArgumentException_Containing_ArgumentException_Containing_Length()
        {
            string longDescription = "";

            foreach (var _ in Enumerable.Range(1, 101))
                longDescription += "a";

            var command = new PostActivityCommand
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 2, 1),
                ActivityType = ActivityType.BuildComponent,
                Workers = new List<char> { 'a' },
                Description = longDescription
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("length"));
        }

        [Test]
        public void Handle_Parameter_With_ActivityType_0_Throws_ArgumentException_Containing_ArgumentException_Containing_ActivityType()
        {
            var command = new PostActivityCommand
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 2, 1),
                Workers = new List<char> { 'a' },
                Description = "description"
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("ActivityType"));
        }

        [Test]
        public void Handle_With_UnitOfWork_Workers_GetByIdIncludingActivities_Throwing_EntityNotFoundInDatabaseException_Throws_It()
        {
            var command = GetValidPostActivityCommand();

            var exception = new EntityNotFoundInDatabaseException(typeof(Worker), 'a');

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Throws(exception);

            var resultException = Assert.ThrowsAsync<EntityNotFoundInDatabaseException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(exception, Is.EqualTo(resultException));
        }

        [Test]
        public async Task Handle_With_UnitOfWork_Workers_GetByIdIncludingActivities_Throwing_EntityNotFoundInDatabaseException_LogsError_And_LogsDebug()
        {
            var command = GetValidPostActivityCommand();

            var exception = new EntityNotFoundInDatabaseException(typeof(Worker), 'a');

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Throws(exception);

            try
            {
                await _handler.Handle(command, CancellationToken.None);
            }
            catch { /**/ }

            _loggerMock.VerifyLogContains(LogLevel.Error, "database");
            _loggerMock.VerifyLogContains(LogLevel.Error, "id a");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "database");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "id a");
        }

        [Test]
        public void Handle_With_UnitOfWork_Workers_GetByIdIncludingActivities_Throwing_TycoonFactorySchedulerAggregationException_Throws_It()
        {
            var command = GetValidPostActivityCommand();

            var exception = new TycoonFactorySchedulerAggregationException(
                new EntityNotFoundInDatabaseException(typeof(Worker), 'a'),
                new EntityNotFoundInDatabaseException(typeof(Worker), 'b'));

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Throws(exception);

            var resultException = Assert.ThrowsAsync<TycoonFactorySchedulerAggregationException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(exception, Is.EqualTo(resultException));
        }

        [Test]
        public async Task Handle_With_UnitOfWork_Workers_GetByIdIncludingActivities_Throwing_TycoonFactorySchedulerAggregationException_LogsError_And_LogsDebug()
        {
            var command = GetValidPostActivityCommand();

            var exception = new TycoonFactorySchedulerAggregationException(
                new EntityNotFoundInDatabaseException(typeof(Worker), 'a'),
                new EntityNotFoundInDatabaseException(typeof(Worker), 'b'));

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Throws(exception);

            try
            {
                await _handler.Handle(command, CancellationToken.None);
            }
            catch { /**/ }

            _loggerMock.VerifyLogContains(LogLevel.Error, "database");
            _loggerMock.VerifyLogContains(LogLevel.Error, "id a");
            _loggerMock.VerifyLogContains(LogLevel.Error, "id b");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "database");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "id a");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "id b");
        }

        [Test]
        public void Handle_With_Activity_AddWorkers_Throwing_ScheduleConflictException_Throws_It()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> { new Worker('a', "a1", "a2", DateTime.Now) };
            var exception = new ScheduleConflictException(
                workers[0],
                new BuildComponentActivity("description1", DateTime.Now, DateTime.Now),
                new BuildComponentActivity("description2", DateTime.Now, DateTime.Now));
            
            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _activityMock.Setup(x => x.AddWorkers(It.IsAny<IEnumerable<Worker>>(), true)).Throws(exception);            
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);
            
            var resultException = Assert.ThrowsAsync<ScheduleConflictException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(exception, Is.EqualTo(resultException));
        }

        [Test]
        public async Task Handle_With_Activity_AddWorkers_Throwing_ScheduleConflictException_LogsError_And_LogsDebug()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> { new Worker('a', "a1", "a2", DateTime.Now) };
            var exception = new ScheduleConflictException(
                workers[0],
                new BuildComponentActivity("description1", DateTime.Now, DateTime.Now),
                new BuildComponentActivity(2, "description2", DateTime.Now, DateTime.Now));

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _activityMock.Setup(x => x.AddWorkers(It.IsAny<IEnumerable<Worker>>(), true)).Throws(exception);
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);

            try
            {
                await _handler.Handle(command, CancellationToken.None);
            }
            catch { /**/ }

            _loggerMock.VerifyLogContains(LogLevel.Error, "activity you're trying to create");
            _loggerMock.VerifyLogContains(LogLevel.Error, "worker a");
            _loggerMock.VerifyLogContains(LogLevel.Error, "activity 2");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "activity you're trying to create");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "worker a");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "activity 2");
        }

        [Test]
        public void Handle_With_Activity_AddWorkers_Throwing_TycoonFactorySchedulerAggregationException_Throws_It()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> {
                new Worker('a', "a1", "a2", DateTime.Now),
                new Worker('b', "b1", "b2", DateTime.Now)
            };

            command.Workers = workers.Select(x => x.Id).ToList();

            var exception = new TycoonFactorySchedulerAggregationException(
                new ScheduleConflictException(
                    workers[0],
                    new BuildComponentActivity("description1", DateTime.Now, DateTime.Now),
                    new BuildComponentActivity(2, "description2", DateTime.Now, DateTime.Now)),
                new ScheduleConflictException(
                    workers[1],
                    new BuildComponentActivity("description1", DateTime.Now, DateTime.Now),
                    new BuildComponentActivity(3, "description2", DateTime.Now, DateTime.Now)));

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _activityMock.Setup(x => x.AddWorkers(It.IsAny<IEnumerable<Worker>>(), true)).Throws(exception);
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);

            var resultException = Assert.ThrowsAsync<TycoonFactorySchedulerAggregationException>(() =>
                 _handler.Handle(command, CancellationToken.None));

            Assert.That(exception, Is.EqualTo(resultException));
        }

        [Test]
        public async Task Handle_With_Activity_AddWorkers_Throwing_TycoonFactorySchedulerAggregationException_LogsError_And_LogsDebug()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> { 
                new Worker('a', "a1", "a2", DateTime.Now),
                new Worker('b', "b1", "b2", DateTime.Now)
            };

            command.Workers = workers.Select(x => x.Id).ToList();

            var exception = new TycoonFactorySchedulerAggregationException(
                new ScheduleConflictException(
                    workers[0],
                    new BuildComponentActivity("description1", DateTime.Now, DateTime.Now),
                    new BuildComponentActivity(2, "description2", DateTime.Now, DateTime.Now)),
                new ScheduleConflictException(
                    workers[1],
                    new BuildComponentActivity("description1", DateTime.Now, DateTime.Now),
                    new BuildComponentActivity(3, "description2", DateTime.Now, DateTime.Now)));

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _activityMock.Setup(x => x.AddWorkers(It.IsAny<IEnumerable<Worker>>(), true)).Throws(exception);
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);

            try
            {
                await _handler.Handle(command, CancellationToken.None);
            }
            catch { /**/ }

            _loggerMock.VerifyLogContains(LogLevel.Error, "activity you're trying to create");
            _loggerMock.VerifyLogContains(LogLevel.Error, "worker a");
            _loggerMock.VerifyLogContains(LogLevel.Error, "activity 2");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "activity you're trying to create");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "worker a");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "activity 2");
            _loggerMock.VerifyLogContains(LogLevel.Error, "worker b");
            _loggerMock.VerifyLogContains(LogLevel.Error, "activity 3");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "worker b");
            _loggerMock.VerifyLogContains(LogLevel.Debug, "activity 3");
        }

        [Test]
        public async Task Handle_Ok_Returns_CreatedObject()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> {
                new Worker('a', "a1", "a2", DateTime.Now),
                new Worker('b', "b1", "b2", DateTime.Now)
            };
            
            command.Workers = workers.Select(x => x.Id).ToList();

            var response = GetValidPostActivityCommandResponse();

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);
            _mapperMock.Setup(x => x.Map<PostActivityCommandResponse>(It.IsAny<object>())).Returns(response);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.EqualTo(response));
        }

        [Test]
        public async Task Handle_Ok_Calls_UnitOfWork_Activities_Insert()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> {
                new Worker('a', "a1", "a2", DateTime.Now),
                new Worker('b', "b1", "b2", DateTime.Now)
            };

            command.Workers = workers.Select(x => x.Id).ToList();

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);

            await _handler.Handle(command, CancellationToken.None);

            _activitiesRepositoryMock.Verify(x => x.Insert(_activityMock.Object));
        }

        [Test]
        public async Task Handle_Ok_Calls_UnitOfWork_SaveAsync()
        {
            var command = GetValidPostActivityCommand();

            var workers = new List<Worker> {
                new Worker('a', "a1", "a2", DateTime.Now),
                new Worker('b', "b1", "b2", DateTime.Now)
            };

            command.Workers = workers.Select(x => x.Id).ToList();

            _workersRepositoryMock.Setup(x => x.GetByIdIncludingActivities(It.IsAny<IEnumerable<char>>())).Returns(workers.AsQueryable());
            _mapperMock.Setup(x => x.Map<Activity>(command)).Returns(_activityMock.Object);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(x => x.SaveAsync());
        }


        private static PostActivityCommandResponse GetValidPostActivityCommandResponse() =>
            new()
            {
                ActivityType = ActivityType.BuildMachine,
                Description = "description",
                Id = 1,
                Workers = new List<char> { 'a' },
                Start = DateTime.Now,
                End = DateTime.Now
            };

        private static PostActivityCommand GetValidPostActivityCommand() =>
            new()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                ActivityType = ActivityType.BuildMachine,
                Description = "description",
                Workers = new List<char> { 'a' }
            };
    }
}
