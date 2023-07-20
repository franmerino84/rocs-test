using MediatR;
using NLog.Extensions.Logging;
using TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy;
using TycoonFactoryScheduler.Infrastructure.Ioc;
using TycoonFactoryScheduler.Infrastructure.Logging;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Workers;

namespace TycoonFactoryScheduler.Presentation.Api.Ioc
{
    public static class ApiDependenciesRegistrar
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDependenciesFromAssemblyOfType<WorkersController>();
            services.AddMappings();
            services.AddMediatR(x =>x.RegisterServicesFromAssembly(typeof(GetTopBusyWorkersQuery).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                });
            });
            return services;
        }
    }
}
