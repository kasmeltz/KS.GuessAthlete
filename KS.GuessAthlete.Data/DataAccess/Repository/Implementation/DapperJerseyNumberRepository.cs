using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to JerseyNumber data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperJerseyNumberRepository : BaseDapperRepository<JerseyNumber>,
        IJerseyNumberRepository
    {        
        public DapperJerseyNumberRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "JerseyNumber";
            TableName = "[app].[JerseyNumber]";
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
                Id, AthleteId, TeamIdentityId, Number, StartYear, EndYear
            FROM 
                [app].[JerseyNumber]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, Number, StartYear, EndYear
            FROM 
                [app].[JerseyNumber]
            ORDER BY
                AthleteId";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, Number, StartYear, EndYear
            FROM 
                [app].[JerseyNumber]
            WHERE           
                AthleteId like @SearchTerms
            ORDER BY
                AthleteId";

        private const string _insertSql = @"
            SET NOCOUNT ON;
	        DECLARE @ExistingId	int;
	        SET @ExistingId = NULL;

	        SELECT TOP 1
		        @ExistingId = Id
	        FROM	
		        [app].[JerseyNumber]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                TeamIdentityId = @TeamIdentityId
            AND 
                StartYear = @StartYear
            AND
                Number = @Number
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[JerseyNumber]
		        (AthleteId, TeamIdentityId, Number, StartYear, EndYear)
		        VALUES
		        (@AthleteId, @TeamIdentityId, @Number, @StartYear, @EndYear)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[JerseyNumber]
	            WHERE	
		            AthleteId = @AthleteId
                AND
                    TeamIdentityId = @TeamIdentityId
                AND 
                    StartYear = @StartYear
                AND
                    Number = @Number
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
		        [app].[JerseyNumber]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                TeamIdentityId = @TeamIdentityId
            AND 
                StartYear = @StartYear
            AND
                Number = @Number

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[JerseyNumber]
                SET
                    AthleteId = @AthleteId,
                    TeamIdentityId = @TeamIdentityId,
                    Number = @Number,
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
