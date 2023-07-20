using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF;
using TycoonFactoryScheduler.Presentation.Api.Ioc;

namespace TycoonFactoryScheduler.Presentation.Api.Configuration
{
    public static class WebApplicationBuilderConfigurer
    {
        public static WebApplicationBuilder GetConfiguredWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TycoonFactorySchedulerContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString(TycoonFactorySchedulerContext.ConnectionStringName)));

            builder.Services.AddDependencies();

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            return builder;
        }
    }
}
