namespace KS.GuessAthlete.Data.POCO
{
    public class StatLine : PocoDataObject
    {
        public int SeasonId { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
    }
}
