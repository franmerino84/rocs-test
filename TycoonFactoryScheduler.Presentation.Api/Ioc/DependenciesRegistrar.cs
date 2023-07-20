using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Ioc;
using TycoonFactoryScheduler.Infrastructure.Ioc;

namespace TycoonFactoryScheduler.Presentation.Api.Ioc    
{
    public static class DependenciesRegistrar
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddApiDependencies();
            services.AddPersistenceDependencies();
            services.AddInfrastructureDependencies();
            return services;
        }
    }
}