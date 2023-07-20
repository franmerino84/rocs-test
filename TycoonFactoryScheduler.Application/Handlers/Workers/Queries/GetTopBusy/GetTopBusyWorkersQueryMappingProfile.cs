using AutoMapper;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers.Models;

namespace TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy
{
    public class GetTopBusyWorkersQueryMappingProfile : Profile
    {
        public GetTopBusyWorkersQueryMappingProfile()
        {
            CreateMap<BusyWorker, GetTopBusyWorkersQueryResponseBusyWorker>()
                .ForMember(destination => destination.Id, expression => expression.MapFrom(source => source.Worker.Id));
        }
    }
}
