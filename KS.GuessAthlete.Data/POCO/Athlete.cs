using System.Collections.Generic;

namespace KS.GuessAthlete.Data.POCO
{
    public class Athlete : PocoDataObject
    {
        public string Name { get; set; }        

        public IEnumerable<StatLine> Stats { get; set; }
    }
}
