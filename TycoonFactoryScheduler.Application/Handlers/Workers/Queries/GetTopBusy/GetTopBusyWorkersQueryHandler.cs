using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Application.Exceptions;
using TycoonFactoryScheduler.Infrastructure.Exceptions.Logging;
using TycoonFactoryScheduler.Infrastructure.Validation;

namespace TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy
{
    public class GetTopBusyWorkersQueryHandler : IRequestHandler<GetTopBusyWorkersQuery, GetTopBusyWorkersQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTopBusyWorkersQueryHandler> _logger;

        public GetTopBusyWorkersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetTopBusyWorkersQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetTopBusyWorkersQueryResponse> Handle(GetTopBusyWorkersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Validate(nameof(request));

            try
            {
                var topBusyWorkers = await _unitOfWork.Workers.GetTopBusy(request.Start, request.End, request.Size);

                var croppedTopBusyWorkers = topBusyWorkers.Take((int)request.Size);

                var response = new GetTopBusyWorkersQueryResponse
                {
                    Workers = _mapper.Map<List<GetTopBusyWorkersQueryResponseBusyWorker>>(croppedTopBusyWorkers)
                };

                return response;
            }
            catch (Exception ex)
            {
                throw LoggedExceptionBuilder.GetPlainErrorLoggedException(this, _logger, ex, (x, y) => new UnexpectedApplicationException(x, y));
            }
        }
    }
}
