using System;

namespace KS.GuessAthlete.Data.POCO
{
    public class TeamIdentity : PocoDataObject
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
