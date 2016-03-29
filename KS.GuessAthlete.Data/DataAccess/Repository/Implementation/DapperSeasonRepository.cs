using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Season data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperSeasonRepository : BaseDapperRepository<Season>,
        ISeasonRepository
    {        
        public DapperSeasonRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Season";
            TableName = "[app].[Season]";
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
                Id, LeagueId, Name, StartDate, EndDate, IsPlayoffs
            FROM 
                [app].[Season]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name, StartDate, EndDate, IsPlayoffs
            FROM 
                [app].[Season]
            ORDER BY
                Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name, StartDate, EndDate, IsPlayoffs
            FROM 
                [app].[Season]
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
		        [app].[Season]
	        WHERE	
		        Name = @Name
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Season]
		        (LeagueId, Name, StartDate, EndDate, IsPlayoffs)
		        VALUES
		        (@LeagueId, @Name, @StartDate, @EndDate, @IsPlayoffs)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Season]
	            WHERE	
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
		        [app].[Season]
	        WHERE	
                Name = @Name

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Season]
                SET
                    LeagueId = @LeagueId,
                    Name = @Name,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    IsPlayoffs = @IsPlayoffs                    
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
