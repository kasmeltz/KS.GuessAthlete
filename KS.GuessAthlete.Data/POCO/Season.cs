using System;

namespace KS.GuessAthlete.Data.POCO
{
    public class Season : PocoDataObject
    {
        public int LeagueId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public int IsPlayoffs { get; set; }
    }
}