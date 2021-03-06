﻿namespace KS.GuessAthlete.Data.POCO.Hockey
{
    public class SkaterStatLine : StatLine
    {
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points
        {
            get
            {
                return Goals + Assists;
            }
        }

        public int PlusMinus { get; set; }
        public int PenaltyMinutes { get; set; }
        public int EvenStrengthGoals { get; set; }
        public int PowerPlayGoals { get; set; }
        public int ShortHandedGoals { get; set; }
        public int GameWinningGoals { get; set; }
        public int EvenStrengthAssists { get; set; }
        public int PowerPlayAssists { get; set; }
        public int ShortHandedAssists { get; set; }
        public int Shots { get; set; }
        public decimal ShotPercentage { get; set; }
        public int TimeOnIce { get; set; }
        public decimal AverageTimeOnIce { get; set; }
        public int StanleyCup { get; set; }
    }
}
