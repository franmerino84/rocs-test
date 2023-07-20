using AutoMapper;
using TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post;
using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Post
{
    public class PostActivityMappingProfile : Profile
    {
        public PostActivityMappingProfile()
        {
            CreateMap<PostActivityRequestDto, PostActivityCommand>()
                .ForMember(command => command.ActivityType, 
                    context => context
                        .MapFrom(dto => Enum.Parse(typeof(ActivityType), dto.ActivityType)));

            CreateMap<PostActivityCommandResponse, PostActivityResponseDto>()
                .ForMember(dto=>dto.ActivityType,
                    context=> context
                        .MapFrom(response=>response.ActivityType.ToString()));

        }
    }
}
