namespace KS.GuessAthlete.Data.POCO
{
    public class TeamIdentityDivision : PocoDataObject
    {
        public int TeamIdentityId { get; set;}
        public int DivisionId { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
