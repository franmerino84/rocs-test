using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;
using TycoonFactoryScheduler.Infrastructure.Uri;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Post;

namespace TycoonFactoryScheduler.Test.Presentation.Api.Controllers.Activities.Post
{
    [TestFixture]
    [Category(Category.Unit)]
    public class ActivitiesControllerPostTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ICreatedUriBuilder> _createdUriBuilderMock;
        public ActivitiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _createdUriBuilderMock = new Mock<ICreatedUriBuilder>();
            _controller = new ActivitiesController(_mediatorMock.Object, _mapperMock.Object, _createdUriBuilderMock.Object);
        }

        [Test]
        public async Task Post_Calls_Mediator_Send_With_MappedParameters()
        {
            var request = GetValidRequestDto();
            var command = GetValidCommand();
            _mapperMock.Setup(x => x.Map<PostActivityCommand>(request)).Returns(command);
            try
            {
                await _controller.Post(request);
            }
            catch {/**/}

            _mediatorMock.Verify(x => x.Send(command, default));
        }

        [Test]
        public async Task Post_With_Mediator_Returning_Returns_Created_WithMappedResult()
        {
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();
            var response = GetValidCommandResponse();
            var responseDto = GetValidResponseDto();

            _mapperMock.Setup(x => x.Map<PostActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(response);

            _mapperMock.Setup(x => x.Map<PostActivityResponseDto>(response)).Returns(responseDto);

            _createdUriBuilderMock.Setup(x => x.Build(_controller, 1)).Returns($"https://url:1234/api/activities/1");

            var result = await _controller.Post(requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<CreatedResult>());
                Assert.That(((CreatedResult)result).Value, Is.EqualTo(responseDto));
            });
        }

        [Test]
        public async Task Post_With_Mediator_Throwing_TycoonFactorySchedulerAggregationException_With_ScheduleConflictExceptions_Returns_Conflict_With_ListOf_Messages()
        {
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();
            var responseBody = new List<string> {
                "Cannot assign the activity 1 to the worker a because it conflicts with the activity 2",
                "Cannot assign the activity 3 to the worker z because it conflicts with the activity 4" };
            var exception = new TycoonFactorySchedulerAggregationException(
                new ScheduleConflictException(
                    new Worker('a', "a1", "a2", DateTime.Now),
                    new BuildComponentActivity(1, "b1", DateTime.Now, DateTime.Now),
                    new BuildComponentActivity(2, "c1", DateTime.Now, DateTime.Now)),
                new ScheduleConflictException(
                    new Worker('z', "z1", "z2", DateTime.Now),
                    new BuildComponentActivity(3, "y1", DateTime.Now, DateTime.Now),
                    new BuildComponentActivity(4, "w1", DateTime.Now, DateTime.Now)));

            _mapperMock.Setup(x => x.Map<PostActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Post(requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
                Assert.That(((ConflictObjectResult)result).Value, Is.Not.Null);
                ((ConflictObjectResult)result).Value?.AssertJsonEqualsTo(responseBody);
            });
        }

        [Test]
        public async Task Post_With_Mediator_Throwing_ScheduleConflictException_Returns_Conflict_With_ListOf_Message()
        {
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();

            var exception = new ScheduleConflictException(
                new Worker('a', "a1", "a2", DateTime.Now),
                new BuildComponentActivity(1, "b", DateTime.Now, DateTime.Now),
                new BuildComponentActivity(2, "c", DateTime.Now, DateTime.Now));

            var expected = new List<string> { "Cannot assign the activity 1 to the worker a because it conflicts with the activity 2" };

            _mapperMock.Setup(x => x.Map<PostActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Post(requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
                Assert.That(((ConflictObjectResult)result).Value, Is.Not.Null);
                ((ConflictObjectResult)result).Value?.AssertJsonEqualsTo(expected);
            });
        }

        [Test]
        public async Task Post_With_Mediator_Throwing_TycoonFactorySchedulerAggregationException_With_EntityNotFoundInDatabase_Returns_NotFound_With_ListOf_Messages()
        {
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();
            var responseBody = new List<string> {
                "The entity of type Worker with id A wasn't found in the database",
                "The entity of type Worker with id B wasn't found in the database" };
            var exception = new TycoonFactorySchedulerAggregationException(
                new EntityNotFoundInDatabaseException(
                    typeof(Worker),
                    "A"),
                new EntityNotFoundInDatabaseException(
                    typeof(Worker),
                    "B"));

            _mapperMock.Setup(x => x.Map<PostActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Post(requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
                Assert.That(((NotFoundObjectResult)result).Value, Is.Not.Null);
                ((NotFoundObjectResult)result).Value?.AssertJsonEqualsTo(responseBody);
            });
        }

        [Test]
        public async Task Post_With_Mediator_Throwing_EntityNotFoundInDatabaseException_Returns_NotFound_With_Message()
        {
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();

            var exception = new EntityNotFoundInDatabaseException(typeof(Worker), 'A');

            var expected = new List<string> { "The entity of type Worker with id A wasn't found in the database" };

            _mapperMock.Setup(x => x.Map<PostActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Post(requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
                Assert.That(((NotFoundObjectResult)result).Value, Is.Not.Null);
                Assert.That(((NotFoundObjectResult)result).Value, Is.EqualTo(expected));

            });
        }

        [Test]
        public async Task Post_With_Mediator_Throwing_UnexpectedApplicationException_Returns_InternalServerError()
        {
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();

            _mapperMock.Setup(x => x.Map<PostActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).Throws<UnexpectedApplicationException>();

            var result = await _controller.Post(requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<StatusCodeResult>());
                Assert.That(((StatusCodeResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            });
        }

        private static PostActivityResponseDto GetValidResponseDto() =>
            new()
            {
                ActivityType = "BuildComponent",
                Description = "description",
                Workers = new List<char> { 'A' },
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                Id = 1
            };

        private static PostActivityCommandResponse GetValidCommandResponse() =>
            new()
            {
                ActivityType = ActivityType.BuildComponent,
                Description = "description",
                Workers = new List<char> { 'A' },
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                Id = 1
            };

        private static PostActivityCommand GetValidCommand() =>
            new()
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                ActivityType = ActivityType.BuildComponent,
                Description = "description",
                Workers = new List<char>() { 'A' },
            };

        private static PostActivityRequestDto GetValidRequestDto() =>
            new()
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                ActivityType = "BuildComponent",
                Description = "description",
                Workers = new List<char> { 'A' }
            };
    }
}
