using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.POCO
{
    public class Draft : PocoDataObject
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Year { get; set; }
        public int Round { get; set; }
        public int Position { get; set; }        
    }
}
