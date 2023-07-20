using AutoMapper;
using TycoonFactoryScheduler.Application.Mapping;
using TycoonFactoryScheduler.Presentation.Api.Mapping;

namespace TycoonFactoryScheduler.Presentation.Api.Ioc
{
    public static class MapperRegistrar
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            IMapper mapper = CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration mappingConfig = new(mc =>
            {
                mc.AddApiProfiles();
                mc.AddApplicationProfiles();
            });

            return mappingConfig.CreateMapper();
        }



    }
}