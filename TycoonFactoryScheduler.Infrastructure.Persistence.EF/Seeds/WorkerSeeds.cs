using System.Collections.Immutable;
using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Seeds
{
    public static class WorkerSeeds
    {
        public static readonly Worker WallE = new('A', "WALL-E", "WALL-E", new DateTime(2008, 6, 21));
        public static readonly Worker NS5 = new('B', "NS-5", "I, Robot", new DateTime(2004, 7, 16));
        public static readonly Worker T800 = new('C', "T-800", "Terminator", new DateTime(1984, 10, 26));
        public static readonly Worker T1000 = new('D', "T-1000", "Terminator", new DateTime(1991, 7, 3));
        public static readonly Worker Cavil = new('E', "Cavil", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Leoben = new('F', "Leoben", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker DAnna = new('G', "D'Anna", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Simon = new('H', "Simon", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Aaron = new('I', "Aaron", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Caprica = new('J', "Caprica", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Daniel = new('K', "Daniel", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Boomer = new('L', "Boomer", "Battlestar Galactica", new DateTime(2003, 12, 8));
        public static readonly Worker Dolores = new('M', "Dolores", "Westworld", new DateTime(2016, 10, 2));
        public static readonly Worker Maeve = new('N', "Maeve", "Westworld", new DateTime(2016, 10, 2));
        public static readonly Worker Bernard = new('O', "Bernard", "Westworld", new DateTime(2016, 10, 2));
        public static readonly Worker Hector = new('P', "Hector", "Westworld", new DateTime(2016, 10, 2));
        public static readonly Worker Teddy = new('Q', "Teddy", "Westworld", new DateTime(2016, 10, 2));
        public static readonly Worker Nexus = new('R', "Nexus", "Blade Runner", new DateTime(1982, 8, 21));
        public static readonly Worker Baymax = new('S', "Baymax", "Big Hero 6", new DateTime(2014, 12, 19));
        public static readonly ImmutableList<Worker> AllWorkers = new List<Worker>()
        {
            WallE,NS5,T800,T1000,Cavil,Leoben,DAnna,Simon,Aaron,Caprica,Daniel,Boomer,Dolores,
            Maeve,Bernard,Hector,Teddy,Nexus,Baymax
        }.ToImmutableList();
    }
}
