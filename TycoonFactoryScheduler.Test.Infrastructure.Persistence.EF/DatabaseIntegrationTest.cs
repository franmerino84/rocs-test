using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace TycoonFactoryScheduler.Test.Infrastructure.Persistence.EF
{

    public abstract class DatabaseIntegrationTest
    {
        protected TycoonFactorySchedulerContext Context { get; set; }

        private ServiceProvider? _serviceProvider; 
        
        protected virtual void SetUp()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            ServiceCollection services = new();
            services.AddDbContext<TycoonFactorySchedulerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(TycoonFactorySchedulerContext.ConnectionStringName)));

            _serviceProvider = services.BuildServiceProvider();
            InitializeContext();
            Context.Database.EnsureDeleted();
            Context.Database.Migrate();
        }

        protected void InitializeContext()
        {
            var options = (_serviceProvider?.GetService<DbContextOptions<TycoonFactorySchedulerContext>>()) ?? throw new TestException();

            Context = new TycoonFactorySchedulerContext(options);
        }

        protected virtual void TearDown() => 
            Context.Database.EnsureDeleted();
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.