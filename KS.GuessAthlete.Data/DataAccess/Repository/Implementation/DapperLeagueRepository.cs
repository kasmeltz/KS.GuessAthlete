using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to League data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperLeagueRepository : BaseDapperRepository<League>,
        ILeagueRepository
    {        
        public DapperLeagueRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "League";
            TableName = "[app].[League]";
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
                Id, Name, Abbreviation
            FROM 
                [app].[League]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, Name, Abbreviation
            FROM 
                [app].[League]
            ORDER BY
                Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, Name, Abbreviation
            FROM 
                [app].[League]
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
		        [app].[League]
	        WHERE	
		        Name = @Name
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[League]
		        (Name, Abbreviation)
		        VALUES
		        (@Name, @Abbreviation)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[League]
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
		        [app].[League]
	        WHERE	
                Name = @Name

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[League]
                SET
                    Name = @Name,
                    Abbreviation = @Abbreviation
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
