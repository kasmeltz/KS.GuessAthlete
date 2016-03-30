using System;

namespace KS.GuessAthlete.Data.POCO
{
    public class Award : PocoDataObject
    {
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
