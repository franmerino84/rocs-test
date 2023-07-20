using Microsoft.Extensions.DependencyInjection;
using TycoonFactoryScheduler.Infrastructure.Uri;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Activities;

namespace TycoonFactoryScheduler.Infrastructure.Ioc
{
    public static class InfrastructureDependenciesRegistrar
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ICreatedUriBuilder, CreatedUriBuilder>();
            return services;
        }
    }
}