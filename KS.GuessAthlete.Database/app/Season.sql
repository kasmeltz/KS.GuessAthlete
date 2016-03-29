CREATE TABLE [app].[Season]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[LeagueId] INT NOT NULL,
	[Name] VARCHAR(255) NULL,
	[StartDate] DATETIME NULL,
	[EndDate] DATETIME NULL,
	[IsPlayoffs] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Season_To_League] FOREIGN KEY ([LeagueId]) REFERENCES [app].[League]([Id])
)


GO
CREATE NONCLUSTERED INDEX [IX_Season_LeagueId]
    ON [app].[Season]([LeagueId] ASC);