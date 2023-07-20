using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TycoonFactoryScheduler.Infrastructure.Uri;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICreatedUriBuilder _createdUriBuilder;

        public ActivitiesController(IMediator mediator, IMapper mapper, ICreatedUriBuilder createdUriBuilder) : base()
        {
            _mediator = mediator;
            _mapper = mapper;
            _createdUriBuilder = createdUriBuilder;
        }
    }
}
