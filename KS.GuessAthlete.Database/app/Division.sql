CREATE TABLE [app].[Division]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[ConferenceId] INT NOT NULL,
	[Name] VARCHAR(255) NULL,
	[StartYear] INT NOT NULL,
	[EndYear] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Division_To_Conference] FOREIGN KEY ([ConferenceId]) REFERENCES [app].[Conference]([Id]),
)

GO
CREATE NONCLUSTERED INDEX [IX_Division_Conference]
    ON [app].[Division]([ConferenceId] ASC);