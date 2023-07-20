using MediatR;
using Microsoft.Extensions.Logging;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post;
using TycoonFactoryScheduler.Infrastructure.Exceptions.Logging;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Delete
{
    public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteActivityCommandHandler> _logger;

        public DeleteActivityCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteActivityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                _unitOfWork.Activities.Delete(request.Id);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Activity {Id} is deleted", request.Id);
            }
            catch(Exception ex)
            {
                throw LoggedExceptionBuilder.GetPlainErrorLoggedException(this, _logger, ex, (x, y) => new UnexpectedApplicationException(x, y));
            }
        }

        
    }
}
