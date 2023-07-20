using AutoMapper;
using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch
{
    public class PatchActivityCommandMappingProfile : Profile
    {
        public PatchActivityCommandMappingProfile()
        {
            CreateMap<Activity, PatchActivityCommandResponse>()
                .ForMember(response => response.Workers, context =>
                    context.MapFrom(activity =>
                        activity.Workers.Select(worker => worker.Id)));


        }
    }
}
