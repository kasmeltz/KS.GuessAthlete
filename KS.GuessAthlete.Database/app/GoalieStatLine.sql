CREATE TABLE [app].[GoalieStatLine]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[SeasonId] INT NOT NULL,
	/*        
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
        public string StanleyCup { get; set; }
        public int IsPlayoffs { get; set; }
		*/
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_GoalieStatLine_To_Athlete] FOREIGN KEY ([AthleteId]) REFERENCES [app].[Athlete]([Id]),
	CONSTRAINT [FK_GoalieStatLine_To_TeamIdentity] FOREIGN KEY ([TeamIdentityId]) REFERENCES [app].[TeamIdentity]([Id]),
	CONSTRAINT [FK_GoalieStatLine_To_Season] FOREIGN KEY ([SeasonId]) REFERENCES [app].[Season]([Id])
)

GO
CREATE NONCLUSTERED INDEX [IX_GoalieStatLine_AthleteId]
    ON [app].[GoalieStatLine]([AthleteId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_GoalieStatLine_TeamIdentityId]
    ON [app].[GoalieStatLine]([TeamIdentityId] ASC);
	
GO
CREATE NONCLUSTERED INDEX [IX_GoalieStatLine_SeasonId]
    ON [app].[GoalieStatLine]([SeasonId] ASC);