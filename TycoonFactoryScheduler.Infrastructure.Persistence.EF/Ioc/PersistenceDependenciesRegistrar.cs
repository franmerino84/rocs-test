using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Infrastructure.Ioc;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Ioc
{
    public static class PersistenceDependenciesRegistrar
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>(serviceProvider=>
                new UnitOfWork(serviceProvider.GetRequiredService<DbContextOptions<TycoonFactorySchedulerContext>>()));

            services.AddDependenciesFromAssemblyOfType<UnitOfWork>();

            return services;
        }
    }
}
