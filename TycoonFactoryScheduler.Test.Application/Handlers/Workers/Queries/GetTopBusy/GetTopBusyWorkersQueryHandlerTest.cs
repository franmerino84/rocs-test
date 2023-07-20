using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers.Models;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy;
using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Test.Application.Handlers.Workers.Queries.GetTopBusy
{
    [TestFixture]
    [Category(Category.Unit)]
    public class GetTopBusyWorkersQueryHandlerTest
    {
        private Mock<IWorkersRepository> _workersRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<GetTopBusyWorkersQueryHandler>> _loggerMock;
        private GetTopBusyWorkersQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _workersRepositoryMock = new Mock<IWorkersRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetTopBusyWorkersQueryHandler>>();
            _handler = new GetTopBusyWorkersQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Handle_Null_Throws_Null_Exception()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                 _handler.Handle(null, CancellationToken.None));
        }


        [Test]
        public void Handle_Parameter_With_Size_LowerThan_One_Throws_ArgumentException_Containing_ArgumentException_Containing_Zero()
        {
            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(new GetTopBusyWorkersQuery(), CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("zero"));
        }

        [Test]
        public void Handle_Parameter_With_End_Before_Start_Throws_ArgumentException_Containing_ArgumentException_Containing_After()
        {
            var query = new GetTopBusyWorkersQuery
            {
                Start = new DateTime(2023, 2, 1),
                End = new DateTime(2023, 1, 1),
                Size = 1
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(query, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<ArgumentException>());
            Assert.That(argumentException.InnerException.Message, Does.Contain("after"));
        }

        [Test]
        public void Handle_Parameter_With_Multiple_Errors_Contains_AggregateException_Containing_ArgumentExceptions()
        {
            var query = new GetTopBusyWorkersQuery
            {
                Start = new DateTime(2023, 2, 1),
                End = new DateTime(2023, 1, 1),
                Size = 0
            };

            var argumentException = Assert.ThrowsAsync<ArgumentException>(() =>
                 _handler.Handle(query, CancellationToken.None));

            Assert.That(argumentException.InnerException, Is.InstanceOf<AggregateException>());

            var aggregateException = (AggregateException)argumentException.InnerException;

            Assert.That(aggregateException?.InnerExceptions.Any(x => x is ArgumentException), Is.True);
        }

        [Test]
        public void Handle_With_UnitOfWorkWorkers_GetTopBusy_Throwing_Throws_UnexpectedApplicationException()
        {
            var query = GetValidQuery();

            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);
            _workersRepositoryMock.Setup(x => x.GetTopBusy(query.Start, query.End, query.Size)).Throws<Exception>();

            Assert.That(async () => await _handler.Handle(query, CancellationToken.None), Throws.TypeOf<UnexpectedApplicationException>());
        }

        [Test]
        public async Task Handle_With_UnitOfWorkWorkers_GetTopBusy_Throwing_LogsDebug_It()
        {
            var query = GetValidQuery();
            var exception = new Exception();
            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);
            _workersRepositoryMock.Setup(x => x.GetTopBusy(query.Start, query.End, query.Size)).Throws(exception);

            try
            {
                await _handler.Handle(query, CancellationToken.None);
            }
            catch (Exception) { /**/ }

            _loggerMock.VerifyLog(LogLevel.Debug, exception.ToString());
        }

        [Test]
        public async Task Handle_With_UnitOfWorkWorkers_GetTopBusy_Throwing_LogsError_Message()
        {
            var query = GetValidQuery();
            var exception = new Exception();
            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);
            _workersRepositoryMock.Setup(x => x.GetTopBusy(query.Start, query.End, query.Size)).Throws(exception);

            try
            {
                await _handler.Handle(query, CancellationToken.None);
            }
            catch (Exception) { /**/ }

            _loggerMock.VerifyLog(LogLevel.Error);
        }

        [Test]
        public async Task Handle_With_Size_GreaterThan_UnitOfWorkWorkers_GetTopBusy_Result_Returns_MappedResult()
        {
            var query = GetValidQuery();
            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);

            var repositoryResult = new List<BusyWorker>
            {
                new BusyWorker(new Worker('A',"a","a",DateTime.Now), TimeSpan.FromHours(3)),
                new BusyWorker(new Worker('B',"B","B",DateTime.Now), TimeSpan.FromHours(2)),
                new BusyWorker(new Worker('B',"B","B",DateTime.Now), TimeSpan.FromHours(1)),
            };

            _workersRepositoryMock.Setup(x => x.GetTopBusy(query.Start, query.End, query.Size)).ReturnsAsync(repositoryResult);

            var expectedResponse = GetMappedResponse(repositoryResult);

            _mapperMock.Setup(x => x.Map<List<GetTopBusyWorkersQueryResponseBusyWorker>>(It.IsAny<object>())).Returns(expectedResponse.Workers);

            var response = await _handler.Handle(query, CancellationToken.None);

            response.AssertJsonEqualsTo(expectedResponse);
        }

        [Test]
        public async Task Handle_With_Size_EqualsTo_UnitOfWorkWorkers_GetTopBusy_Result_Returns_MappedResult()
        {
            var query = GetValidQuery();
            query.Size = 3;

            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);

            var repositoryResult = new List<BusyWorker>
            {
                new BusyWorker(new Worker('A',"a","a",DateTime.Now), TimeSpan.FromHours(3)),
                new BusyWorker(new Worker('B',"B","B",DateTime.Now), TimeSpan.FromHours(2)),
                new BusyWorker(new Worker('B',"B","B",DateTime.Now), TimeSpan.FromHours(1)),
            };

            _workersRepositoryMock.Setup(x => x.GetTopBusy(query.Start, query.End, query.Size)).ReturnsAsync(repositoryResult);

            var expectedResponse = GetMappedResponse(repositoryResult);

            _mapperMock.Setup(x => x.Map<List<GetTopBusyWorkersQueryResponseBusyWorker>>(It.IsAny<object>())).Returns(expectedResponse.Workers);

            var response = await _handler.Handle(query, CancellationToken.None);

            response.AssertJsonEqualsTo(expectedResponse);
        }

        [Test]
        public async Task Handle_With_Size_LowerTo_UnitOfWorkWorkers_GetTopBusy_Result_Returns_MappedResult_CroppedTo_Size_LastElements()
        {
            var query = GetValidQuery();
            query.Size = 2;

            _unitOfWorkMock.Setup(x => x.Workers).Returns(_workersRepositoryMock.Object);

            var repositoryResult = new List<BusyWorker>
            {
                new BusyWorker(new Worker('A',"C","C",DateTime.Now), TimeSpan.FromHours(3)),
                new BusyWorker(new Worker('B',"B","B",DateTime.Now), TimeSpan.FromHours(2)),
                new BusyWorker(new Worker('C',"C","C",DateTime.Now), TimeSpan.FromHours(1)),
            };

            _workersRepositoryMock.Setup(x => x.GetTopBusy(query.Start, query.End, query.Size)).ReturnsAsync(repositoryResult);

            var expectedResponse = GetMappedResponse(repositoryResult);

            expectedResponse.Workers = expectedResponse.Workers.Take((int)query.Size).ToList();

            _mapperMock.Setup(x => x.Map<List<GetTopBusyWorkersQueryResponseBusyWorker>>(It.IsAny<object>())).Returns(expectedResponse.Workers);

            var response = await _handler.Handle(query, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Workers, Has.Count.EqualTo(query.Size));
                Assert.That(response.Workers.Select(x => x.Id), Does.Not.Contains('C'));
            });
        }

        private static GetTopBusyWorkersQueryResponse GetMappedResponse(List<BusyWorker> repositoryResult) =>
            new()
            {
                Workers = repositoryResult.Select(x => new GetTopBusyWorkersQueryResponseBusyWorker
                {
                    Id = x.Worker.Id,
                    TimeBusy = x.TimeBusy
                }).ToList()
            };

        private static GetTopBusyWorkersQuery GetValidQuery() =>
            new()
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 8, 1),
                Size = 10
            };
    }
}
