/**********************
   Initial data load
**********************/
/* Leagues */
DECLARE @NHLLeagueId INT
IF NOT EXISTS (SELECT Id FROM [app].[League] WHERE Name = 'National Hockey League')
BEGIN
	INSERT INTO 
		[app].[League]
		(Name, Abbreviation)
	VALUES
		('National Hockey League', 'NHL');

	SELECT @NHLLeagueId = SCOPE_IDENTITY();
END;

SELECT 
	@NHLLeagueId = Id 
FROM 
	[app].[League] 
WHERE 
	Name = 'National Hockey League';

/* Seasons */
IF NOT EXISTS (SELECT Id FROM [app].[Season] WHERE Name = '1905-1906 NHL Regular Season')
BEGIN
	DECLARE @cnt INT = 1905;
	WHILE @cnt < 2030
	BEGIN
		INSERT INTO 
		[app].[Season]
		(LeagueId, Name, StartDate, EndDate, IsPlayoffs)
		VALUES
		(@NHLLeagueId, 
			CONVERT(varchar(4), @cnt) + '-' + CONVERT(varchar(4), (@cnt + 1)) + ' NHL Regular Season', 
			CONVERT(varchar(4), @cnt) + '-09-01', 
			CONVERT(varchar(4), (@cnt + 1)) + '-04-10', 
			0)

		INSERT INTO 
		[app].[Season]
		(LeagueId, Name, StartDate, EndDate, IsPlayoffs)
		VALUES
		(@NHLLeagueId, 
		CONVERT(varchar(4), @cnt) + '-' + CONVERT(varchar(4), (@cnt + 1)) + ' NHL Playoffs', 
		CONVERT(varchar(4), (@cnt + 1)) + '-04-14', 
		CONVERT(varchar(4), (@cnt + 1)) + '-06-02', 
		1)

	   SET @cnt = @cnt + 1;
	END;
END;

/* Awards */
IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'All Star Voting')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'All Star Voting', 'AS', '1930-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Hart Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Hart Memorial Trophy', 'Hart', '1923-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Lady Byng Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Lady Byng Memorial Trophy', 'Byng', '1924-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Vezina Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Vezina Trophy', 'Vezina', '1926-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Calder Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Calder Memorial Trophy', 'Calder', '1936-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Art Ross Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Art Ross Trophy', 'Ross', '1947-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'James Norris Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'James Norris Memorial Trophy', 'Norris', '1953-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Conn Smythe Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Conn Smythe', 'Smythe', '1964-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Bill Masterton Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Bill Masterton Memorial Trophy', 'Masterton', '1967-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Ted Lindsay Award')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Ted Lindsay Award', 'Lindsay', '1970-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Frank J. Selke Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Frank J. Selke Trophy', 'Selke', '1977-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'William M. Jennings Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'William M. Jennings Trophy', 'Jennings', '1981-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'King Clancy Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'King Clancy Memorial Trophy', 'Clancy', '1987-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Maurice Richard Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Maurice Richard Trophy', 'Richard', '1998-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Jack Adams Award')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Jack Adams Award', 'Adams', '1973-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Stanley Cup')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Stanley Cup', 'Stanley', '1917-01-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Presidents Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Presidents Trophy', 'Presidents', '1917-01-01', null);
END;

/* Teams */
DECLARE @TeamId INT;
IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = '')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, '');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'National Hockey League');
END;

/* Initial Users */
/*
IF NOT EXISTS (SELECT Id FROM [dbo].[AspNetUsers] WHERE UserName = 'superadmin')
BEGIN
	INSERT INTO 
		[dbo].[AspNetUsers]
		(Id, 
		Email, EmailConfirmed, 
		PasswordHash, 
		SecurityStamp, 
		PhoneNumber, PhoneNumberConfirmed, 
		TwoFactorEnabled, 
		LockoutEndDateUtc, LockoutEnabled, 
		AccessFailedCount, 
		UserName)
	VALUES
		('652487ea-7db6-4863-afcb-f5545053bddd',
		'kasmeltz@lakeheadu.ca', 0, 
		'ALXgsqgOavMF0NlqKOPZclMB/0jjdKf+Ngt7PsYQr05tJvi6Tv1X63AS5CAMEYKohg==', 
		'4cf5086e-e4a6-4dae-982a-b2eecfbd64a0',
		NULL, 0,
		0,
		NULL, 0,
		0,
		'superadmin');

	INSERT INTO
		[app].[UserProfile]
		(UserId, FirstName, MiddleName, LastName)
		VALUES
		('652487ea-7db6-4863-afcb-f5545053bddd', 'Super', 'KS', 'Admin');
END;
*/