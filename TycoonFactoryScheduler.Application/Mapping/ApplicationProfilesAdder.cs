using AutoMapper;
using TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy;
using TycoonFactoryScheduler.Infrastructure.Mapping;

namespace TycoonFactoryScheduler.Application.Mapping
{
    public static class ApplicationProfilesAdder
    {
        public static void AddApplicationProfiles(this IMapperConfigurationExpression mapperConfigurationExpression) =>
            mapperConfigurationExpression.AddProfilesFromAssemblyOfType<GetTopBusyWorkersQueryHandler>();
    }
}
