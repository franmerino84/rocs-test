using AutoMapper;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Patch
{
    public class PatchActivityMappingProfile : Profile
    {
        public PatchActivityMappingProfile()
        {
            CreateMap<PatchActivityRequestDto, PatchActivityCommand>();

            CreateMap<PatchActivityCommandResponse, PatchActivityResponseDto>()
                .ForMember(dto => dto.ActivityType,
                    context => context
                        .MapFrom(response => response.ActivityType.ToString()));

        }
    }
}
