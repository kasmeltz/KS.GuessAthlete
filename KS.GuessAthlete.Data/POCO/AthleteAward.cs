namespace KS.GuessAthlete.Data.POCO
{
    public class AthleteAward : PocoDataObject
    {
        public int AwardId { get; set; }
        public int AthleteId { get; set; }
        public int SeasonId { get; set; }
        public int Position { get; set; }
    }
}
