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

/* Seasons */
IF NOT EXISTS (SELECT Id FROM [app].[Season] WHERE Name = '1905-1906 NHL Regular Season')
BEGIN
	DECLARE @cnt INT = 1905;

	WHILE @cnt < cnt_total
	BEGIN
		INSERT INTO 
		[app].[Season]
		(LeagueId, Name, StartDate, EndDate, IsPlayoffs)
		VALUES
		(@NHLLeagueId, @cnt + '-' + (@cnt + 1) + ' NHL Regular Season', '1905-09-01', '1906-04-10', 0)

	   SET @cnt = @cnt + 1;
	END;
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