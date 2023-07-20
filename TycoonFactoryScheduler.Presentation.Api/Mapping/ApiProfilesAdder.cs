using AutoMapper;
using TycoonFactoryScheduler.Infrastructure.Mapping;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Workers;

namespace TycoonFactoryScheduler.Presentation.Api.Mapping
{
    public static class ApiProfilesAdder
    {
        public static void AddApiProfiles(this IMapperConfigurationExpression mapperConfigurationExpression) =>
            mapperConfigurationExpression.AddProfilesFromAssemblyOfType<WorkersController>();
    }
}
