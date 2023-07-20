using Microsoft.Extensions.DependencyInjection;

namespace TycoonFactoryScheduler.Infrastructure.Ioc
{
    public interface IDependenciesRegistrar
    {
        void Register(IServiceCollection service);
    }
}
