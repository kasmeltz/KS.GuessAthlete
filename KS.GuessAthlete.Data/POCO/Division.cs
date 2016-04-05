namespace KS.GuessAthlete.Data.POCO
{
    public class Division : PocoDataObject
    {
        public int ConferenceId { get; set;}
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
