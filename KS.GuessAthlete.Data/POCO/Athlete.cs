using System;
using System.Collections.Generic;

namespace KS.GuessAthlete.Data.POCO
{
    public class Athlete : PocoDataObject
    {
        public string Name { get; set; }        
        public DateTime BirthDate { get; set; }
        public string BirthCountry { get; set; }
        public string BirthCity { get; set; }        
        public string Position { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }

        public IEnumerable<StatLine> Stats { get; set; }
        public IEnumerable<Draft> Drafts { get; set; }
        public IEnumerable<JerseyNumber> JerseyNumbers { get; set; }
    }
}
