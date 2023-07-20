using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;
using TycoonFactoryScheduler.Infrastructure.Uri;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Patch;

namespace TycoonFactoryScheduler.Test.Presentation.Api.Controllers.Activities.Patch
{
    [TestFixture]
    [Category(Category.Unit)]
    public class ActivitiesControllerPatchTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IMapper> _mapperMock;
        public ActivitiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ActivitiesController(_mediatorMock.Object, _mapperMock.Object, Mock.Of<ICreatedUriBuilder>());
        }

        [Test]
        public async Task Patch_Calls_Mediator_Send_With_MappedParameters()
        {
            var id = 1;
            var request = GetValidRequestDto();
            var command = GetValidCommand();

            _mapperMock.Setup(x => x.Map<PatchActivityCommand>(request)).Returns(command);

            await _controller.Patch(id, request);

            _mediatorMock.Verify(x => x.Send(command, default));
        }

        [Test]
        public async Task Patch_With_Mediator_Returning_Returns_Ok_WithMappedResult()
        {
            var id = 1;
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();
            var response = GetValidCommandResponse();
            var responseDto = GetValidResponseDto();

            _mapperMock.Setup(x => x.Map<PatchActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(response);

            _mapperMock.Setup(x => x.Map<PatchActivityResponseDto>(response)).Returns(responseDto);

            var result = await _controller.Patch(id, requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(responseDto));
            });
        }

        [Test]
        public async Task Patch_With_Mediator_Throwing_TycoonFactorySchedulerAggregationException_Returns_Conflict_With_ListOf_Messages()
        {
            var id = 1;
            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();
            var responseBody = new List<string> { "a", "b" };
            var exception = new TycoonFactorySchedulerAggregationException(
                new Exception("a"), new Exception("b") );

            _mapperMock.Setup(x => x.Map<PatchActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Patch(id, requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
                Assert.That(((ConflictObjectResult)result).Value, Is.Not.Null);
                ((ConflictObjectResult)result).Value?.AssertJsonEqualsTo(responseBody);
            });
        }

        [Test]
        public async Task Patch_With_Mediator_Throwing_ScheduleConflictException_Returns_Conflict_With_ListOf_Message()
        {
            var id = 1;

            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();

            var exception = new ScheduleConflictException(
                new Worker('a', "a1", "a2", DateTime.Now),
                new BuildComponentActivity(1, "b", DateTime.Now, DateTime.Now),
                new BuildComponentActivity(2, "c", DateTime.Now, DateTime.Now));

            var expected = new List<string> { "Cannot assign the activity 1 to the worker a because it conflicts with the activity 2" };

            _mapperMock.Setup(x => x.Map<PatchActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Patch(id, requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
                Assert.That(((ConflictObjectResult)result).Value, Is.Not.Null);
                ((ConflictObjectResult)result).Value?.AssertJsonEqualsTo(expected);
            });
        }

        [Test]
        public async Task Patch_With_Mediator_Throwing_EntityNotFoundInDatabaseException_Returns_NotFound_With_Message()
        {
            var id = 1;

            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();

            var exception = new EntityNotFoundInDatabaseException(
                typeof(Activity),
                id);

            var expected = "The entity of type Activity with id 1 wasn't found in the database";

            _mapperMock.Setup(x => x.Map<PatchActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).ThrowsAsync(exception);

            var result = await _controller.Patch(id, requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
                Assert.That(((NotFoundObjectResult)result).Value, Is.Not.Null);
                Assert.That(((NotFoundObjectResult)result).Value, Is.EqualTo(expected));

            });
        }

        [Test]
        public async Task Patch_With_Mediator_Throwing_UnexpectedApplicationException_Returns_InternalServerError()
        {
            var id = 1;

            var requestDto = GetValidRequestDto();
            var command = GetValidCommand();

            _mapperMock.Setup(x => x.Map<PatchActivityCommand>(requestDto)).Returns(command);

            _mediatorMock.Setup(x => x.Send(command, default)).Throws<UnexpectedApplicationException>();

            var result = await _controller.Patch(id, requestDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<StatusCodeResult>());
                Assert.That(((StatusCodeResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            });
        }


        private static PatchActivityResponseDto GetValidResponseDto() =>
            new()
            {
                ActivityType = "BuildComponent",
                Description = "description",
                Workers = new List<char> { 'A' },
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                Id = 1
            };

        private static PatchActivityCommandResponse GetValidCommandResponse() =>
            new()
            {
                ActivityType = ActivityType.BuildComponent,
                Description = "description",
                Workers = new List<char> { 'A' },
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                Id = 1
            };

        private static PatchActivityCommand GetValidCommand() =>
            new()
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2),
                Id = 1
            };

        private static PatchActivityRequestDto GetValidRequestDto() =>
            new()
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 2)
            };
    }
}
