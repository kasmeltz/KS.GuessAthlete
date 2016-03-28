using System.Collections.Generic;

namespace KS.GuessAthlete.Data.POCO
{
    public class Athlete
    {
        public string Name { get; set; }

        public IEnumerable<AggregateStat> Stats { get; set; }
    }
}
