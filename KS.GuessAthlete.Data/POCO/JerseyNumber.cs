using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.POCO
{
    public class JerseyNumber : PocoDataObject
    {
        public int AthleteId { get; set; }
        public string TeamName { get; set; }
        public int TeamId { get; set; }
        public int Number { get; set; }
        public string Years { get; set; }

    }
}
