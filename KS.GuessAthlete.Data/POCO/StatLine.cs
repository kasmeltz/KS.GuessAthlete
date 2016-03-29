namespace KS.GuessAthlete.Data.POCO
{
    public class StatLine : PocoDataObject
    {
        public int AthleteId { get; set; }
        public int SeasonId { get; set; }
        public int TeamIdentityId { get; set; }

        public string Season { get; set; }
        public string Year { get; set; }
        public string TeamAbbreviation { get; set; }
        public string TeamName { get; set; }
    }
}