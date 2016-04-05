using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Conference data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperConferenceRepository : BaseDapperRepository<Conference>,
        IConferenceRepository
    {        
        public DapperConferenceRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Conference";
            TableName = "[app].[Conference]";
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

        private const string _getSql = @"
            SET NOCOUNT ON;
            SELECT TOP 1
                Id, LeagueId, Name, StartYear, EndYear
            FROM 
                [app].[Conference]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name, StartYear, EndYear
            FROM 
                [app].[Conference]
            ORDER BY
                StartYear, Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name, StartYear, EndYear
            FROM 
                [app].[Conference]
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
		        [app].[Conference]
	        WHERE	
		        Name = @Name
            AND
                StartYear = @StartYear
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Conference]
		        (LeagueId, Name, StartYear, EndYear)
		        VALUES
		        (@LeagueId, @Name, @StartYear, @EndYear)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Conference]
	            WHERE	
		            Name = @Name
                AND
                    StartYear = @StartYear       
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
		        [app].[Conference]
	        WHERE	
		        Name = @Name
            AND
                StartYear = @StartYear

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Conference]
                SET
                    LeagueId = @LeagueId,
                    Name = @Name,                    
                    StartYear = @StartYear,
                    EndYear = @EndYear
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
