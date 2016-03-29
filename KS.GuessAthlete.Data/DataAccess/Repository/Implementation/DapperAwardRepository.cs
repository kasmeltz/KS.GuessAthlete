using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Award data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperAwardRepository : BaseDapperRepository<Award>,
        IAwardRepository
    {        
        public DapperAwardRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Award";
            TableName = "[app].[Award]";
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
                Id, LeagueId, Name, Abbreviation, StartDate, EndDate
            FROM 
                [app].[Award]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name, Abbreviation, StartDate, EndDate
            FROM 
                [app].[Award]
            ORDER BY
                Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name, Abbreviation, StartDate, EndDate
            FROM 
                [app].[Award]
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
		        [app].[Award]
	        WHERE	
		        LeagueId = @LeagueId
            AND
                Name = @Name
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Award]
		        (LeagueId, Name, Abbreviation, StartDate, EndDate)
		        VALUES
		        (@LeagueId, @Name, @Abbreviation, @StartDate, @EndDate)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Award]
	            WHERE	
		            LeagueId = @LeagueId
                AND
                    Name = @Name       
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
		        [app].[Award]
	        WHERE	
		        LeagueId = @LeagueId
            AND
                Name = @Name

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Award]
                SET
                    LeagueId = @LeagueId,
                    Name = @Name,
                    Abbreviation = @Abbreviation,
                    StartDate = @StartDate,
                    EndDate = @EndDate
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
