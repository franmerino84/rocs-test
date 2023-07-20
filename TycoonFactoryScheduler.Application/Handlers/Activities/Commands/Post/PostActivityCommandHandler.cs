using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Common.Exceptions;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Infrastructure.Exceptions.Logging;
using TycoonFactoryScheduler.Infrastructure.Validation;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post
{
    public class PostActivityCommandHandler : IRequestHandler<PostActivityCommand, PostActivityCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PostActivityCommandHandler> _logger;

        public PostActivityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostActivityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostActivityCommandResponse> Handle(PostActivityCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Validate(nameof(request));

            try
            {
                var workers = _unitOfWork.Workers.GetByIdIncludingActivities(request.Workers);

                var activity = _mapper.Map<Activity>(request);

                activity.AddWorkers(workers.AsEnumerable());

                _unitOfWork.Activities.Insert(activity);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created activity {Id} of type {ActivityType}, assigned to {Workers} with description \"{Description}\" ",
                    activity.Id, activity.GetActivityType().ToString(), string.Join(',', request.Workers), activity.Description);

                return _mapper.Map<PostActivityCommandResponse>(activity);
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
