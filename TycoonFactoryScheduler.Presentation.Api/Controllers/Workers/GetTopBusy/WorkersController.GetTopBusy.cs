using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Workers
{
    public partial class WorkersController : ControllerBase
    {
        [HttpGet]
        [Route("top-busy")]
        [ProducesResponseType(typeof(GetTopBusyWorkersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopBusy([FromQuery] GetTopBusyWorkersRequestDto requestDto)
        {
            try
            {
                var query = _mapper.Map<GetTopBusyWorkersQuery>(requestDto);

                var result = await _mediator.Send(query);

                var responseDto = _mapper.Map<GetTopBusyWorkersResponseDto>(result);

                responseDto.Workers = responseDto.Workers.Take(requestDto.Size).ToList();

                return Ok(responseDto);
            }
            catch (UnexpectedApplicationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
