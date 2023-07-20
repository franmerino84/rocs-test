using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Workers;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy;

namespace TycoonFactoryScheduler.Test.Presentation.Api.Controllers.Workers.GetTopBusy
{
    [TestFixture]
    [Category(Category.Unit)]
    public class GetTopBusyWorkersControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IMapper> _mapperMock;
        private WorkersController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _controller = new WorkersController(_mediatorMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetTopBusy_WithParameters_Calls_Mediator_Send_With_SameMappedParams()
        {
            var dto = new GetTopBusyWorkersRequestDto()
            {
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 20),
                Size = 30
            };

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);            

            var response = new GetTopBusyWorkersQueryResponse();

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            MockResponseMapping(response);

            await _controller.GetTopBusy(dto);

            _mediatorMock.Verify(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GetTopBusy_With_Mediator_Returning_Result_Returns_StatusCode_Ok()
        {
            var dto = new GetTopBusyWorkersRequestDto();

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);

            var response = new GetTopBusyWorkersQueryResponse();

            MockResponseMapping(response);

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetTopBusy(dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetTopBusy_With_Mediator_Returning_Result_Returns_StatusCode_Ok_With_MappedResponseDto()
        {
            var dto = new GetTopBusyWorkersRequestDto();

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);

            var response = new GetTopBusyWorkersQueryResponse();

            GetTopBusyWorkersResponseDto responseDto = MockResponseMapping(response);

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetTopBusy(dto);

            var innerObject = ((OkObjectResult)result).Value;

            Assert.That(innerObject, Is.EqualTo(responseDto));
        }

        [Test]
        public async Task GetTopBusy_With_Mediator_Returning_Result_WithSameSize_Than_Parameter_Returns_SameMappedResult()
        {
            var dto = new GetTopBusyWorkersRequestDto { Size = 3 };

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);

            var response = new GetTopBusyWorkersQueryResponse
            {
                Workers = new List<GetTopBusyWorkersQueryResponseBusyWorker>
                {
                    new GetTopBusyWorkersQueryResponseBusyWorker(),
                    new GetTopBusyWorkersQueryResponseBusyWorker(),
                    new GetTopBusyWorkersQueryResponseBusyWorker(),
                }
            };

            MockResponseMapping(response);

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetTopBusy(dto);

            GetTopBusyWorkersResponseDto? innerObject = (GetTopBusyWorkersResponseDto?)((OkObjectResult)result).Value;

            if (innerObject != null)
                Assert.That(innerObject.Workers, Has.Count.EqualTo(dto.Size));
            else
                Assert.Fail();
        }

        [Test]
        public async Task GetTopBusy_With_Mediator_Returning_Result_WithLowerSize_Than_Parameter_Returns_SameMappedResult()
        {
            var dto = new GetTopBusyWorkersRequestDto { Size = 5 };

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);

            var response = new GetTopBusyWorkersQueryResponse
            {
                Workers = new List<GetTopBusyWorkersQueryResponseBusyWorker>
                {
                    new GetTopBusyWorkersQueryResponseBusyWorker(),
                    new GetTopBusyWorkersQueryResponseBusyWorker(),
                    new GetTopBusyWorkersQueryResponseBusyWorker(),
                }
            };

            MockResponseMapping(response);

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetTopBusy(dto);

            GetTopBusyWorkersResponseDto? innerObject = (GetTopBusyWorkersResponseDto?)((OkObjectResult)result).Value;

            if (innerObject != null)
                Assert.That(innerObject.Workers, Has.Count.EqualTo(response.Workers.Count));
            else
                Assert.Fail();
        }

        [Test]
        public async Task GetTopBusy_With_Mediator_Returning_Result_WithHigherSize_Than_Parameter_Returns_SameMappedResult_CroppedTo_Parameter_Size_First_Elements()
        {
            var dto = new GetTopBusyWorkersRequestDto { Size = 2 };

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);

            var response = new GetTopBusyWorkersQueryResponse
            {
                Workers = new List<GetTopBusyWorkersQueryResponseBusyWorker>
                {
                    new GetTopBusyWorkersQueryResponseBusyWorker{ Id = 'A' },
                    new GetTopBusyWorkersQueryResponseBusyWorker{ Id = 'B' },
                    new GetTopBusyWorkersQueryResponseBusyWorker{ Id = 'C' }
                }
            };

            MockResponseMapping(response);

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetTopBusy(dto);

            GetTopBusyWorkersResponseDto? innerObject = (GetTopBusyWorkersResponseDto?)((OkObjectResult)result).Value;

            if (innerObject != null)
            {
                Assert.That(innerObject.Workers, Has.Count.EqualTo(dto.Size));
                Assert.That(innerObject.Workers.Select(x => x.Id), Does.Not.Contains('C'));
            }
            else
                Assert.Fail();
        }

        [Test]
        public async Task GetTopBusy_With_Mediator_Throwing_UnhandledException_Returns_StatusCode_InternalServerError()
        {
            var dto = new GetTopBusyWorkersRequestDto();

            GetTopBusyWorkersQuery getTopBusyQuery = MockQueryMapping(dto);

            _mediatorMock.Setup(x => x.Send(getTopBusyQuery, It.IsAny<CancellationToken>())).ThrowsAsync(new UnexpectedApplicationException());

            var result = await _controller.GetTopBusy(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<StatusCodeResult>());
                Assert.That(((StatusCodeResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            });
        }

        private GetTopBusyWorkersResponseDto MockResponseMapping(GetTopBusyWorkersQueryResponse response)
        {
            var workers = response.Workers.Select(x => new GetTopBusyWorkersResponseDtoBusyWorker
            {
                Id = x.Id,
                TimeBusy = x.TimeBusy
            }).ToList();

            var responseDto = new GetTopBusyWorkersResponseDto()
            {
                Workers = workers
            };

            _mapperMock.Setup(x => x.Map<GetTopBusyWorkersResponseDto>(response))
                .Returns(responseDto);

            return responseDto;
        }

        private GetTopBusyWorkersQuery MockQueryMapping(GetTopBusyWorkersRequestDto dto)
        {
            var getTopBusyQuery = new GetTopBusyWorkersQuery()
            {
                Start = dto.Start,
                End = dto.End,
                Size = (uint)dto.Size
            };

            _mapperMock.Setup(x => x.Map<GetTopBusyWorkersQuery>(dto))
                .Returns(getTopBusyQuery);

            return getTopBusyQuery;
        }

    }
}
