using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF
{
    public class TycoonFactorySchedulerContext : DbContext
    {
        public const string ConnectionStringName = "TycoonFactoryScheduler";

        public TycoonFactorySchedulerContext(DbContextOptions<TycoonFactorySchedulerContext> options) : base(options) { }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
               .HasDiscriminator<ActivityType>(nameof(BuildComponentActivity.ActivityType))               
               .HasValue<BuildComponentActivity>(BuildComponentActivity.ActivityType)
               .HasValue<BuildMachineActivity>(BuildMachineActivity.ActivityType);
            
            modelBuilder.ApplySeed();
        }
    }
}
