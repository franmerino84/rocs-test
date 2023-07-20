using System.Collections.Immutable;
using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds.Activities
{
    public static class BuildMachineActivitySeeds
    {
        public static readonly BuildMachineActivity AutonomousCar = new(

            8,
            "Autonomous car",
            Seed.ReferenceStartDate.AddHours(12),
            Seed.ReferenceStartDate.AddHours(17)
        );

        public static readonly BuildMachineActivity TimeMachine = new(

            9,
            "Time machine",
            Seed.ReferenceStartDate.AddHours(4),
            Seed.ReferenceStartDate.AddHours(11)
        );

        public static readonly BuildMachineActivity Battlestar = new(

            10,
            "Battlestar",
            Seed.ReferenceStartDate.AddHours(6),
            Seed.ReferenceStartDate.AddHours(8)
        );


        public static readonly BuildMachineActivity FlyingCar = new(

            11,
            "Flying car",
            Seed.ReferenceStartDate.AddHours(7),
            Seed.ReferenceStartDate.AddHours(23)
        );


        public static readonly BuildMachineActivity LaserGun = new(

            12,
            "Laser gun",
            Seed.ReferenceStartDate.AddHours(5),
            Seed.ReferenceStartDate.AddHours(9)
        );


        public static readonly BuildMachineActivity AugmentedRealityGlasses = new(

            13,
            "Augmented reality glasses",
            Seed.ReferenceStartDate.AddHours(10),
            Seed.ReferenceStartDate.AddHours(20)
        );


        public static readonly BuildMachineActivity Teletransporter = new(

            14,
            "Teletransporter", Seed.ReferenceStartDate.AddHours(27),
            Seed.ReferenceStartDate.AddHours(42)
        );


        public static readonly BuildMachineActivity LaserSword = new(

            15,
            "Laser sword",
            Seed.ReferenceStartDate.AddHours(24),
            Seed.ReferenceStartDate.AddHours(28)
        );


        public static readonly BuildMachineActivity Hoverboard = new(

            16,
            "Hoverboard",
            Seed.ReferenceStartDate.AddHours(17),
            Seed.ReferenceStartDate.AddHours(25)
        );

        public static readonly ImmutableList<BuildMachineActivity> AllBuildMachineActivities = new List<BuildMachineActivity>()
        {
            AutonomousCar, TimeMachine, Battlestar,FlyingCar,LaserGun,AugmentedRealityGlasses,Teletransporter,LaserSword,Hoverboard
        }.ToImmutableList();
    }
}
