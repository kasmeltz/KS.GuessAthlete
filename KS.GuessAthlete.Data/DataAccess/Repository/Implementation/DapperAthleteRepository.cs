using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Athlete data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperAthleteRepository : BaseDapperRepository<Athlete>,
        IAthleteRepository
    {        
        public DapperAthleteRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Athlete";
            TableName = "[app].[Athlete]";
            CacheSeconds = 3600;
        }

        protected override void CreateSql()
        {
            GetSql = _getSql;
            ListSql = _listSql;
            SearchSql = _searchSql;
            InsertSql = _insertSql;
            UpdateSql = _updateSql;
        }

        private const string _skatersForCriteriaSql = @"
            SET NOCOUNT ON;
            SELECT
	            ath.Id
            FROM
	            [app].[Athlete] ath
            INNER JOIN
	            [app].[SkaterStatLine] skt
            ON
	            skt.AthleteId = ath.Id
            INNER JOIN
	            [app].[Season] sea
            ON
	            skt.SeasonId = sea.Id
            GROUP BY
	            ath.Id
            HAVING
	            SUM(skt.GamesPlayed) >= @GamesPlayed
            AND
	            SUM(skt.Goals) + SUM(skt.Assists) >= @Points
            AND
	            CAST(SUM(skt.Goals) + SUM(skt.Assists) as decimal) /
	            CAST(SUM(skt.GamesPlayed) as decimal) >= @PPG
            AND
	            YEAR(MIN(sea.StartDate)) >= @StartYear";

        public Task<IEnumerable<int>> SkatersForCriteria(int gamesPlayed, int points, decimal ppg, int startYear)
        {
            return List<int>(_skatersForCriteriaSql,
                new
                {
                    GamesPlayed = gamesPlayed,
                    Points = points,
                    PPG = ppg,
                    StartYear = startYear
                }, 
                string.Format("SkaterForCriteriagp{0}p{1}ppg{2}sy{3}", gamesPlayed, points, ppg, startYear));
        }

        private const string _goaliesForCriteriaSql = @"
            SET NOCOUNT ON;
            SELECT
	            ath.Id
            FROM
	            [app].[Athlete] ath
            INNER JOIN
	            [app].[GoalieStatLine] gst
            ON
	            gst.AthleteId = ath.Id
            INNER JOIN
	            [app].[Season] sea
            ON
	            gst.SeasonId = sea.Id
            GROUP BY
	            ath.Id 
            HAVING
	            SUM(gst.GamesPlayed) >= @GamesPlayed
            AND
                SUM(gst.Wins) >= @Wins
            AND
	            YEAR(MIN(sea.StartDate)) >= @StartYear";

        public Task<IEnumerable<int>> GoaliesForCriteria(int gamesPlayed, int wins, int startYear)
        {
            return List<int>(_goaliesForCriteriaSql,
                new
                {
                    GamesPlayed = gamesPlayed,
                    Wins = wins,
                    StartYear = startYear
                },
                string.Format("GoalieForCriteriagp{0}w{1}sy{2}", gamesPlayed, wins, startYear));
        }

        private const string _getSql = @"
            SET NOCOUNT ON;
            SELECT TOP 1
                Id, Name, BirthDate, BirthCountry, BirthCity, Position, Height, Weight
            FROM 
                [app].[Athlete]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, Name, BirthDate, BirthCountry, BirthCity, Position, Height, Weight
            FROM 
                [app].[Athlete]
            ORDER BY
                Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, Name, BirthDate, BirthCountry, BirthCity, Position, Height, Weight
            FROM 
                [app].[Athlete]
            WHERE           
                Name like @SearchTerms
            ORDER BY
                Name";

        private const string _insertSql = @"
            SET NOCOUNT ON;
	        DECLARE @ExistingId	int;
	        SET @ExistingId = NULL;

	        SELECT TOP 1
		        @ExistingId = Id
	        FROM	
		        [app].[Athlete]
	        WHERE	
		        Name = @Name
            AND
                (@BirthDate IS NULL OR BirthDate = @BirthDate)
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Athlete]
		        (Name, BirthDate, BirthCountry, BirthCity, Position, Height, Weight)
		        VALUES
		        (@Name, @BirthDate, @BirthCountry, @BirthCity, @Position, @Height, @Weight)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Athlete]
	            WHERE	
		            Name = @Name
                AND
                    (@BirthDate IS NULL OR BirthDate = @BirthDate)
            END
	        ELSE
	        BEGIN
		        SELECT -1
	        END";

        private const string _updateSql = @"
            SET NOCOUNT ON;
            DECLARE @ExistingId	int;
	        SET @ExistingId = NULL;

	        SELECT TOP 1
		        @ExistingId = Id
	        FROM	
		        [app].[Athlete]
	        WHERE	
		        Name = @Name
            AND
                (@BirthDate IS NULL OR BirthDate = @BirthDate)

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Athlete]
                SET
                    Name = @Name,
                    BirthDate = @BirthDate,
                    BirthCountry = @BirthCountry,
                    BirthCity = @BirthCity,
                    Position = @Position,
                    Height = @Height,
                    Weight = @Weight
		        WHERE	
		            Id = @Id
                    
                SELECT @Id
            END
            ELSE
            BEGIN
                SELECT -1
            END";        
    }
}
