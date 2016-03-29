CREATE TABLE [app].[SkaterStatLine]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[SeasonId] INT NOT NULL,
	[GamesPlayed] INT NULL,
	[Goals] INT NULL,
	[Assists] INT NULL,
	[PlusMinus] INT NULL,
	[PenaltyMinutes] INT NULL,
	[EvenStrengthGoals] INT NULL,
	[PowerPlayGoals] INT NULL,
	[ShortHandedGoals] INT NULL,
	[GameWinningGoals] INT NULL,
	[EvenStrengthAssists] INT NULL,
	[PowerPlayAssists] INT NULL,
	[ShortHandedAssists] INT NULL,
	[Shots] INT NULL,
	[ShotPercentage] DECIMAL(9,5) NULL,
	[TimeOnIce] INT NULL,
	[AverageTimeOnIce] DECIMAL(9,5) NULL,
	[StanleyCup] INT NULL,
	[IsPlayoffs] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_SkaterStatLine_To_Athlete] FOREIGN KEY ([AthleteId]) REFERENCES [app].[Athlete]([Id]),
	CONSTRAINT [FK_SkaterStatLine_To_TeamIdentity] FOREIGN KEY ([TeamIdentityId]) REFERENCES [app].[TeamIdentity]([Id]),
	CONSTRAINT [FK_SkaterStatLine_To_Season] FOREIGN KEY ([SeasonId]) REFERENCES [app].[Season]([Id])
)

GO
CREATE NONCLUSTERED INDEX [IX_SkaterStatLine_AthleteId]
    ON [app].[SkaterStatLine]([AthleteId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_SkaterStatLine_TeamIdentityId]
    ON [app].[SkaterStatLine]([TeamIdentityId] ASC);
	
GO
CREATE NONCLUSTERED INDEX [IX_SkaterStatLine_SeasonId]
    ON [app].[SkaterStatLine]([SeasonId] ASC);