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
			0),
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
		(@NHLLeagueId, 'All Star Voting', 'AS', '1930-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Hart Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Hart Memorial Trophy', 'Hart', '1923-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Lady Byng Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Lady Byng Memorial Trophy', 'Byng', '1924-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Vezina Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Vezina Trophy', 'Vezina', '1926-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Calder Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Calder Memorial Trophy', 'Calder', '1936-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Art Ross Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Art Ross Trophy', 'Ross', '1947-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'James Norris Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'James Norris Memorial Trophy', 'Norris', '1953-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Conn Smythe Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Conn Smythe', 'Smythe', '1964-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Bill Masterton Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Bill Masterton Memorial Trophy', 'Masterton', '1967-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Ted Lindsay Award')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Ted Lindsay Award', 'Lindsay', '1970-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Frank J. Selke Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Frank J. Selke Trophy', 'Selke', '1977-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'William M. Jennings Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'William M. Jennings Trophy', 'Jennings', '1981-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'King Clancy Memorial Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'King Clancy Memorial Trophy', 'Clancy', '1987-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Maurice Richard Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Maurice Richard Trophy', 'Richard', '1998-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Jack Adams Award')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Jack Adams Award', 'Adams', '1973-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Stanley Cup')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Stanley Cup', 'Stanley', '1917-09-01', null);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Award] WHERE Name = 'Presidents Trophy')
BEGIN	
	INSERT INTO 
		[app].[Award]
		(LeagueId, Name, Abbreviation, StartDate, EndDate)
	VALUES
		(@NHLLeagueId, 'Presidents Trophy', 'Presidents', '1917-09-01', null);
END;

/* Teams */
DECLARE @TeamId INT;
IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'St. Louis Eagles')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'St. Louis Eagles');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Eagles', 'STE', 'St Louis.', '1934-09-01', '1935-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Pittsburgh Pirates')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Pittsburgh Pirates');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Pirates', 'PTP', 'Pittsburgh', '1925-09-01', '1930-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Philadelphia Quakers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Philadelphia Quakers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Quakers', 'PHQ', 'Philadelphia', '1930-09-01', '1931-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Ottawa Senators 1')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Ottawa Senators 1');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Senators', 'OTS', 'Ottawa', '1917-09-01', '1934-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'New York Americans')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'New York Americans');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Americans', 'NYA', 'New York', '1925-09-01', '1941-07-01'),
		(@TeamId, 'Americans', 'BRO', 'Brooklyn', '1941-09-01', '1942-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Montreal Wanderers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Montreal Wanderers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Wanderers', 'MTW', 'Montreal', '1917-09-01', '1918-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Montreal Maroons')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Montreal Maroons');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Maroons', 'MTM', 'Montreal', '1924-09-01', '1938-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Quebec Atheltic Club')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Quebec Atheltic Club');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Atheltic Club', 'QBC', 'Quebec', '1919-09-01', '1920-07-01'),
		(@TeamId, 'Tigers', 'HAM', 'Hamilton', '1920-09-01', '1925-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Oakland Seals')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Oakland Seals');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Seals', 'OAK', 'Oakland', '1967-09-01', '1970-07-01'),
		(@TeamId, 'Golden Seals', 'CGS', 'California', '1970-09-01', '1976-07-01'),
		(@TeamId, 'Barons', 'CLE', 'Cleveland', '1976-09-01', '1978-07-01');
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Atlanta Thrashers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Atlanta Thrashers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Thrashers', 'ATL', 'Atlanta', '1999-09-01', '2011-07-01'),
		(@TeamId, 'Jets', 'WPG', 'Winnipeg', '2011-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Washington Capitals')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Washington Capitals');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Capitals', 'WSH', 'Washington', '1974-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Vancouver Canucks')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Vancouver Canucks');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Canucks', 'VAN', 'Vancouver', '1970-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Toronto Maple Leafs')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Toronto Maple Leafs');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Arenas', 'TRA', 'Toronto', '1917-09-01', '1919-07-01'),
		(@TeamId, 'St. Patricks', 'TRS', 'Toronto', '1919-09-01', '1926-07-01'),
		(@TeamId, 'Maple Leafs', 'TOR', 'Toronto', '1926-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Tampa Bay Lightning')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Tampa Bay Lightning');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Lightning', 'TBL', 'Tampa Bay', '1992-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'St. Louis Blues')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'St. Louis Blues');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Blues', 'STL', 'St. Louis', '1967-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'San Jose Sharks')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'San Jose Sharks');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Sharks', 'SJS', 'San Jose', '1991-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Pittsburgh Penguins')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Pittsburgh Penguins');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Penguins', 'PIT', 'Pittsburgh', '1967-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Philadelphia Flyers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Philadelphia Flyers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Flyers', 'PHI', 'Philadelphia', '1967-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Ottawa Senators')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Ottawa Senators');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Senators', 'OTT', 'Ottawa', '1992-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'New York Rangers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'New York Rangers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Rangers', 'NYR', 'New York', '1926-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'New York Islanders')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'New York Islanders');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Islanders', 'NYI', 'New York', '1972-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Kansas City Scouts')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Kansas City Scouts');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Scouts', 'KCS', 'Kansas City', '1974-09-01', '1976-07-01'),
		(@TeamId, 'Rockies', 'CLR', 'Colorado', '1976-09-01', '1982-07-01'),
		(@TeamId, 'Devils', 'NJD', 'New Jersey', '1976-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Nashville Predators')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Nashville Predators');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Predators', 'NSH', 'Nashville', '1998-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Montreal Canadians')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Montreal Canadians');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Canadians', 'MTL', 'Montreal', '1917-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Minnesota Wild')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Minnesota Wild');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Wild', 'MIN', 'Minnesota', '2000-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Los Angeles Kings')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Los Angeles Kings');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Kings', 'LAK', 'Los Angeles', '1967-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Florida Panthers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Florida Panthers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Panthers', 'FLA', 'Florida', '1993-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Edmonton Oilers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Edmonton Oilers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Oilers', 'EDM', 'Edmonton', '1979-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Detroit Cougars')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Detroit Cougars');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Cougars', 'DTC', 'Detroit', '1926-09-01', '1930-07-01'),
		(@TeamId, 'Falcons', 'DTC', 'Detroit', '1930-09-01', '1932-07-01'),
		(@TeamId, 'Red Wings', 'DET', 'Detroit', '1932-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Minnesota North Stars')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Minnesota North Stars');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'North Stars', 'MNS', 'Minnesota', '1967-09-01', '1993-07-01'),
		(@TeamId, 'Stars', 'DAL', 'Dallas', '1993-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Columbus Blue Jackets')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Columbus Blue Jackets');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Blue Jackets', 'CBJ', 'Columbus', '2000-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Quebec Nordiques')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Quebec Nordiques');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Nordiques', 'QUE', 'Quebec', '1979-09-01', '1995-07-01'),
		(@TeamId, 'Avalanche', 'COL', 'Colorado', '1995-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Chicago Black Hawks')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Chicago Black Hawks');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Black Hawks', 'CBH', 'Chicago', '1926-09-01', '1986-07-01'),
		(@TeamId, 'Blackhawks', 'CHI', 'Chicago', '1986-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Hartford Whalers')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Hartford Whalers');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Whalers', 'HAR', 'Hartford', '1979-09-01', '1997-07-01'),
		(@TeamId, 'Hurricanes', 'CAR', 'Carolina', '1997-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Atlanta Flames')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Atlanta Flames');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Flames', 'ATF', 'Atlanta', '1972-09-01', '1980-07-01'),
		(@TeamId, 'Flames', 'CGY', 'Calgary', '1980-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Buffalo Sabres')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Buffalo Sabres');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Sabres', 'BUF', 'Buffalo', '1970-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Boston Bruins')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Boston Bruins');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Bruins', 'BOS', 'Boston', '1924-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Winnipeg Jets')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Winnipeg Jets');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Jets', 'WIN', 'Winnipeg', '1979-09-01', '1996-07-01'),
		(@TeamId, 'Coyotes', 'PHX', 'Phoenix', '1996-09-01', '2014-07-01'),
		(@TeamId, 'Coyotes', 'ARI', 'Arizona', '2014-09-01', NULL);
END;

IF NOT EXISTS (SELECT Id FROM [app].[Team] WHERE Name = 'Mighty Ducks of Anaheim')
BEGIN
	INSERT INTO 
		[app].[Team]
		(LeagueId, Name)
	VALUES
		(@NHLLeagueId, 'Mighty Ducks of Anaheim');

	SELECT @TeamId = SCOPE_IDENTITY();

	INSERT INTO 
		[app].[TeamIdentity]
		(TeamId, Name, Abbreviation, City, StartDate, EndDate)
	VALUES
		(@TeamId, 'Mighty Ducks of', 'MDA', 'Anaheim', '1993-09-01', '2006-07-01'),
		(@TeamId, 'Ducks', 'ANA', 'Anaheim', '2006-09-01', NULL);
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