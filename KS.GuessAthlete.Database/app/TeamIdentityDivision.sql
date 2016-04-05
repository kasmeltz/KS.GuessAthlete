CREATE TABLE [app].[TeamIdentityDivision]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[TeamIdentityId] INT NOT NULL,
	[DivisionId] INT NOT NULL,
	[StartYear] INT NOT NULL,
	[EndYear] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_TeamIdentityDivision_To_TeamIdentity] FOREIGN KEY ([TeamIdentityId]) REFERENCES [app].[TeamIdentity]([Id]),
	CONSTRAINT [FK_TeamIdentityDivision_To_Division] FOREIGN KEY ([DivisionId]) REFERENCES [app].[Division]([Id]),
)

GO
CREATE NONCLUSTERED INDEX [IX_TeamIdentityDivision_TeamIdentityId]
    ON [app].[TeamIdentityDivision]([TeamIdentityId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_TeamIdentityDivision_DivisionId]
    ON [app].[TeamIdentityDivision]([DivisionId] ASC);