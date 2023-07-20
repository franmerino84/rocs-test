using Microsoft.AspNetCore.Mvc;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Activities.Exceptions;
using TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Post;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities
{
    public partial class ActivitiesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(PostActivityResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PostActivityRequestDto requestDto)
        {
            try
            {
                var command = _mapper.Map<PostActivityCommand>(requestDto);

                var result = await _mediator.Send(command);

                var responseDto = _mapper.Map<PostActivityResponseDto>(result);

                return Created(_createdUriBuilder.Build(this, responseDto.Id), responseDto);
            }
            catch (TycoonFactorySchedulerAggregationException ex)
            {
                var innerExceptionType = ex.InnerExceptions.First().GetType();

                if (innerExceptionType.Equals(typeof(ScheduleConflictException)))
                    return Conflict(ex.Messages);
                else if (innerExceptionType.Equals(typeof(EntityNotFoundInDatabaseException)))
                    return NotFound(ex.Messages);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (ScheduleConflictException ex)
            {
                return Conflict(new string[] { ex.Message });
            }
            catch (EntityNotFoundInDatabaseException ex)
            {
                return NotFound(new string[] { ex.Message });
            }
            catch (MaximumNumberOfWorkersAllowedExceededException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (MinimumNumberOfWorkersNeededNotMetException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (UnexpectedApplicationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
