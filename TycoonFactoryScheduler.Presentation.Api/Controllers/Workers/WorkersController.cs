using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Workers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class WorkersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public WorkersController(IMediator mediator, IMapper mapper) : base()
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}
