namespace KS.GuessAthlete.Data.POCO
{
    public class Draft : PocoDataObject
    {
        public int AthleteId { get; set; }
        public int TeamIdentityId { get; set; }
        public int Year { get; set; }
        public int Round { get; set; }
        public int Position { get; set; }

        public string TeamName { get; set; }
    }
}
