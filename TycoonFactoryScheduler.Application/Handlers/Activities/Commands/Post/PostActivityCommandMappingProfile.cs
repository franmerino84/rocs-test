using AutoMapper;
using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post
{
    public class PostActivityCommandMappingProfile : Profile
    {
        public PostActivityCommandMappingProfile()
        {
            CreateMap<PostActivityCommand, Activity>()
                .ConstructUsing((command, context) =>
                {
                    return command.ActivityType switch
                    {
                        ActivityType.BuildComponent => new BuildComponentActivity(command.Description, command.Start, command.End),
                        ActivityType.BuildMachine => new BuildMachineActivity(command.Description, command.Start, command.End),
                        _ => throw new NotImplementedException(),
                    };
                }).ForMember(activity => activity.Workers, context => context.Ignore());

            CreateMap<Activity, PostActivityCommandResponse>()
                .ForMember(response => response.Workers, context =>
                    context.MapFrom(activity =>
                        activity.Workers.Select(worker => worker.Id)));


        }
    }
}
