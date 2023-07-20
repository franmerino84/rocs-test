using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Domain.Entities.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds.Activities;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds
{
    public static class Seed
    {
        public static readonly DateTime ReferenceStartDate = new(2023,3,3);

        public static void ApplySeed(this ModelBuilder modelBuilder)
        {
            WorkerSeeds.AllWorkers.ForEach(worker => modelBuilder.Entity<Worker>().HasData(worker));
            BuildComponentActivitySeeds.AllBuildComponentActivities.ForEach(activity => modelBuilder.Entity<BuildComponentActivity>().HasData(activity));
            BuildMachineActivitySeeds.AllBuildMachineActivities.ForEach(activity => modelBuilder.Entity<BuildMachineActivity>().HasData(activity));
            modelBuilder.Entity<Activity>()
                .HasMany(activity => activity.Workers)
                .WithMany(worker => worker.Activities)
                .UsingEntity(j => j.ToTable("ActivityWorker").HasData(ActivityWorkerSeeds.AllActivityWorkers));
        }
    }
}
