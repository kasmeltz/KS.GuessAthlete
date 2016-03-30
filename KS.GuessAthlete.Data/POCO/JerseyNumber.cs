namespace KS.GuessAthlete.Data.POCO
{
    public class JerseyNumber : PocoDataObject
    {
        public int AthleteId { get; set; }
        public int TeamIdentityId { get; set; }
        public int Number { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public string TeamName { get; set; }
        public string Years { get; set; }
    }
}
