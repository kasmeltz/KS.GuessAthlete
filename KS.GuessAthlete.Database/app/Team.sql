CREATE TABLE [app].[Team]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[LeagueId] INT NOT NULL,
	[Name] VARCHAR(255) NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Team_To_League] FOREIGN KEY ([LeagueId]) REFERENCES [app].[League]([Id]),
)

GO
CREATE NONCLUSTERED INDEX [IX_Team_LeagueId]
    ON [app].[Team]([LeagueId] ASC);