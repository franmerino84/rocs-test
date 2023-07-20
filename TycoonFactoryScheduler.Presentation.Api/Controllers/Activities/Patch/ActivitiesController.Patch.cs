using Microsoft.AspNetCore.Mvc;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Patch;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities
{
    public partial class ActivitiesController : ControllerBase
    {

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(typeof(PatchActivityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchActivityRequestDto requestDto)
        {
            try
            {
                var command = _mapper.Map<PatchActivityCommand>(requestDto);

                command.Id = id;

                var result = await _mediator.Send(command);

                var responseDto = _mapper.Map<PatchActivityResponseDto>(result);

                return Ok(responseDto);
            }
            catch (TycoonFactorySchedulerAggregationException ex)
            {
                return Conflict(ex.Messages);
            }
            catch (ScheduleConflictException ex)
            {
                return Conflict(new string[] { ex.Message });
            }
            catch (EntityNotFoundInDatabaseException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnexpectedApplicationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
