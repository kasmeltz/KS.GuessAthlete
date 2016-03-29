CREATE TABLE [app].[Draft]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[Year] INT NULL,
	[Round] INT NULL,
	[Position] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Draft_To_Athlete] FOREIGN KEY ([AthleteId]) REFERENCES [app].[Athlete]([Id]),
	CONSTRAINT [FK_Draft_To_TeamIdentity] FOREIGN KEY ([TeamIdentityId]) REFERENCES [app].[TeamIdentity]([Id])
)

GO
CREATE NONCLUSTERED INDEX [IX_Draft_AthleteId]
    ON [app].[Draft]([AthleteId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Draft_TeamIdentityId]
    ON [app].[Draft]([TeamIdentityId] ASC);