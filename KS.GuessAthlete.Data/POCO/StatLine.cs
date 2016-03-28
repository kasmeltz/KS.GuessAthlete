namespace KS.GuessAthlete.Data.POCO
{
    public class StatLine : PocoDataObject
    {
        public int PlayerId { get; set; }
        public int SeasonId { get; set; }
        public string Season { get; set; }   
        public string Year { get; set; }
        public int TeamId { get; set; }
        public string TeamAbbreviation { get; set; }
        public string TeamName { get; set; }
        public int JerseyNumber { get; set; }        
    }
}
