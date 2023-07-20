using System.Collections.Immutable;
using System.Diagnostics;
using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds.Activities
{
    public static class BuildComponentActivitySeeds
    {
        public static readonly BuildComponentActivity FluxCapacitor = new(
            1,
            "Flux capacitor",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(10)
        );

        public static readonly BuildComponentActivity FusionEngine = new(

            2,
            "Fusion engine",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(2)
        );

        public static readonly BuildComponentActivity BionicArm = new(

            3,
            "Bionic arm",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(4)
        );

        public static readonly BuildComponentActivity ThoughtSensor = new(

            4,
            "Thought sensor",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(5)
        );

        public static readonly BuildComponentActivity InterestellarJumpEngine = new(

            5,
            "Interestellar jump engine",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(3)
        );

        public static readonly BuildComponentActivity SyntheticSkin = new(

            6,
            "Synthetic skin",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(1)
        );

        public static readonly BuildComponentActivity BionicLeg = new(

            7,
            "Bionic leg",
            Seed.ReferenceStartDate,
            Seed.ReferenceStartDate.AddHours(8)
        );

        public static readonly ImmutableList<BuildComponentActivity> AllBuildComponentActivities = new List<BuildComponentActivity>()
        {
            FluxCapacitor,FusionEngine, BionicArm,ThoughtSensor, InterestellarJumpEngine,SyntheticSkin, BionicLeg
        }.ToImmutableList();
    }
}
