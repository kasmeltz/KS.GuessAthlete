namespace KS.GuessAthlete.Data.POCO
{
    public class Conference : PocoDataObject
    {
        public int LeagueId { get; set;}
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
