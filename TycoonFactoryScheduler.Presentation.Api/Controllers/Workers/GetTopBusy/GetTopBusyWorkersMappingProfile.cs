using AutoMapper;
using TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy
{
    public class GetTopBusyWorkersMappingProfile : Profile
    {
        public GetTopBusyWorkersMappingProfile()
        {
            CreateMap<GetTopBusyWorkersRequestDto, GetTopBusyWorkersQuery>();
            CreateMap<GetTopBusyWorkersQueryResponseBusyWorker, GetTopBusyWorkersResponseDtoBusyWorker>();
            CreateMap<GetTopBusyWorkersQueryResponse, GetTopBusyWorkersResponseDto>();
        }
    }
}
