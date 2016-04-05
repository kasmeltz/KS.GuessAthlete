CREATE TABLE [app].[Conference]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[LeagueId] INT NOT NULL,
	[Name] VARCHAR(255) NULL,
	[StartYear] INT NOT NULL,
	[EndYear] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Conference_To_League] FOREIGN KEY ([LeagueId]) REFERENCES [app].[League]([Id]),
)

GO
CREATE NONCLUSTERED INDEX [IX_Conference_LeagueId]
    ON [app].[Conference]([LeagueId] ASC);