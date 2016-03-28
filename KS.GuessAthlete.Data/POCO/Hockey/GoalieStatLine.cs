using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.POCO.Hockey
{
    public class GoalieStatLine : StatLine
    {
        public int GamesPlayed { get; set; }
        public int GamesStarted { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TiesPlusOvertimeShootoutLosses { get; set; }
        public int GoalsAgainst { get; set; }
        public int ShotsAgainst { get; set; }
        public int Saves { get; set; }
        public decimal SavePercentage { get; set; }
        public decimal GoalsAgainstAverage { get; set; }
        public int Shutouts { get; set; }
        public int Minutes { get; set; }
        public int QualityStarts { get; set; }
        public decimal QualityStartPercentage { get; set; }
        public int ReallyBadStarts { get; set; }
        public decimal GoalsAgainstPercentage { get; set; }
        public decimal GoalsSavedAboveAverage { get; set; }
        public decimal GoaliePointShares { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points
        {
            get
            {
                return Goals + Assists;
            }
        }
        public int PenaltyMinutes { get; set; }
        public string Awards { get; set; }
    }
}
