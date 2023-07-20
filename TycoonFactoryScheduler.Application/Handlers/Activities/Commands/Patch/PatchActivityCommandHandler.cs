using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;
using TycoonFactoryScheduler.Infrastructure.Exceptions.Logging;
using TycoonFactoryScheduler.Infrastructure.Validation;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch
{
    public class PatchActivityCommandHandler : IRequestHandler<PatchActivityCommand, PatchActivityCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchActivityCommandHandler> _logger;

        public PatchActivityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PatchActivityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PatchActivityCommandResponse> Handle(PatchActivityCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Validate(nameof(request));

            try
            {
                var activity = _unitOfWork.Activities.GetByIdIncludingWorkersIncludingActivities(request.Id);

                activity.UpdateDates(request.Start, request.End);

                _unitOfWork.Activities.Update(activity);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated activity {Id} start date to {Start}, and end date to {End}",
                    activity.Id, activity.Start.ToString(), activity.End.ToString());

                return _mapper.Map<PatchActivityCommandResponse>(activity);
            }
            catch (TycoonFactorySchedulerAggregationException ex)
            {
                ex.InnerExceptions.ToList().ForEach(x => _logger.LogException(ex));
                throw;
            }
            catch (AutomaticallyLoggableException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                throw LoggedExceptionBuilder.GetPlainErrorLoggedException(this, _logger, ex, (x, y) => new UnexpectedApplicationException(x, y));
            }
        }
    }
}
