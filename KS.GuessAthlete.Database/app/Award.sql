CREATE TABLE [app].[Award]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[LeagueId] INT NOT NULL,
	[Name] VARCHAR(255) NULL,
	[Abbreviation] VARCHAR(255) NULL,
	[StartDate] DATETIME NULL,
	[EndDate] DATETIME NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Award_To_League] FOREIGN KEY ([LeagueId]) REFERENCES [app].[League]([Id])
)

GO
CREATE NONCLUSTERED INDEX [IX_Award_LeagueId]
    ON [app].[Award]([LeagueId] ASC);