CREATE TABLE [app].[TeamIdentity]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[TeamId] INT NOT NULL,
	[Name] VARCHAR(255) NULL,
	[Abbreviation] VARCHAR(255) NULL,
	[City] VARCHAR(255) NULL,
	[StartDate] DATETIME NULL,
	[EndDate] DATETIME NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_TeamIdentity_To_Team] FOREIGN KEY ([TeamId]) REFERENCES [app].[Team]([Id]),
)

GO
CREATE NONCLUSTERED INDEX [IX_Team_Identity_TeamId]
    ON [app].[TeamIdentity]([TeamId] ASC);