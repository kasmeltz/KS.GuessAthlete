CREATE TABLE [app].[GoalieStatLine]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[SeasonId] INT NOT NULL,
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