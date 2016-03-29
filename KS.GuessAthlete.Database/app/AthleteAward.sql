﻿CREATE TABLE [app].[AthleteAward]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,	
	[AwardId] INT NOT NULL,
	[AthleteId] INT NOT NULL,
	[TeamIdentityId] INT NOT NULL,
	[SeasonId] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_AthleteAward_To_Award] FOREIGN KEY ([AwardId]) REFERENCES [app].[Award]([Id]),
	CONSTRAINT [FK_AthleteAward_To_Athlete] FOREIGN KEY ([AthleteId]) REFERENCES [app].[Athlete]([Id]),
	CONSTRAINT [FK_AthleteAward_To_Season] FOREIGN KEY ([SeasonId]) REFERENCES [app].[Season]([Id])
)

GO
CREATE NONCLUSTERED INDEX [IX_AthleteAward_AwardId]
    ON [app].[AthleteAward]([AwardId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_AthleteAward_AthleteId]
    ON [app].[AthleteAward]([AthleteId] ASC);
	
GO
CREATE NONCLUSTERED INDEX [IX_AthleteAward_TeamIdentityId]
    ON [app].[AthleteAward]([TeamIdentityId] ASC);