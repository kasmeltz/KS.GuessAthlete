using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Division data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperDivisionRepository : BaseDapperRepository<Division>,
        IDivisionRepository
    {        
        public DapperDivisionRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Division";
            TableName = "[app].[Division]";
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
                Id, ConferenceId, Name, StartYear, EndYear
            FROM 
                [app].[Division]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, ConferenceId, Name, StartYear, EndYear
            FROM 
                [app].[Division]
            ORDER BY
                StartYear, Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, ConferenceId, Name, StartYear, EndYear
            FROM 
                [app].[Division]
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
		        [app].[Division]
	        WHERE	
		        Name = @Name
            AND
                StartYear = @StartYear
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Division]
		        (ConferenceId, Name, StartYear, EndYear)
		        VALUES
		        (@ConferenceId, @Name, @StartYear, @EndYear)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Division]
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
		        [app].[Division]
	        WHERE	
		        Name = @Name
            AND
                StartYear = @StartYear

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Division]
                SET
                    ConferenceId = @ConferenceId,
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
