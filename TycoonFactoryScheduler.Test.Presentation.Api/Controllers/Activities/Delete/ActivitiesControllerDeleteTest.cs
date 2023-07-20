using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Delete;
using TycoonFactoryScheduler.Infrastructure.Uri;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities;

namespace TycoonFactoryScheduler.Test.Presentation.Api.Controllers.Activities.Delete
{
    [TestFixture]
    [Category(Category.Unit)]
    public class ActivitiesControllerDeleteTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IMapper> _mapperMock;        
        private ActivitiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();

            _controller = new ActivitiesController(_mediatorMock.Object, _mapperMock.Object, Mock.Of<ICreatedUriBuilder>());
        }

        [Test]
        public async Task Delete_Ok_Returns_NoContent()
        {
            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_With_Mediator_Send_Throwing_UnexpectedApplicationException_Returns_InternalServerError()
        {            
            _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteActivityCommand>(), default)).Throws<UnexpectedApplicationException>();

            var result = await _controller.Delete(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<StatusCodeResult>());
                Assert.That(((StatusCodeResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            });
        }

    }
}
