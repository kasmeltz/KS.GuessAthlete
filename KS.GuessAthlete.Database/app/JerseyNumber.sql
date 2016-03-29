CREATE TABLE [app].[JerseyNumber]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[Number] INT NULL,
	[StartYear] INT NULL,
	[EndYear] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JerseyNumber_To_Athlete] FOREIGN KEY ([AthleteId]) REFERENCES [app].[Athlete]([Id]),
	CONSTRAINT [FK_JerseyNumber_To_TeamIdentity] FOREIGN KEY ([TeamIdentityId]) REFERENCES [app].[TeamIdentity]([Id])
)

GO
CREATE NONCLUSTERED INDEX [IX_JerseyNumber_AthleteId]
    ON [app].[JerseyNumber]([AthleteId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_JerseyNumber_TeamIdentityId]
    ON [app].[Draft]([TeamIdentityId] ASC);