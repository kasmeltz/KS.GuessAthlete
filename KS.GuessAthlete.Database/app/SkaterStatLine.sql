CREATE TABLE [app].[SkaterStatLine]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[SeasonId] INT NOT NULL,
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