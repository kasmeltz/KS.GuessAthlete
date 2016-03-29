using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Team data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperTeamRepository : BaseDapperRepository<Team>,
        ITeamRepository
    {        
        public DapperTeamRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Team";
            TableName = "[app].[Team]";
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
                Id, LeagueId, Name
            FROM 
                [app].[Team]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name
            FROM 
                [app].[Team]
            ORDER BY
                Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, LeagueId, Name
            FROM 
                [app].[Team]
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
		        [app].[Team]
	        WHERE	
		        Name = @Name
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Team]
		        (LeagueId, Name)
		        VALUES
		        (@LeagueId, @Name)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Team]
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
		        [app].[Team]
	        WHERE	
                Name = @Name

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Team]
                SET
                    LeagueId = @LeagueId,
                    Name = @Name
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
