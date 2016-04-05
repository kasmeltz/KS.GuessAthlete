using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to TeamIdentityDivision data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperTeamIdentityDivisionRepository : BaseDapperRepository<TeamIdentityDivision>,
        ITeamIdentityDivisionRepository
    {        
        public DapperTeamIdentityDivisionRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "TeamIdentityDivision";
            TableName = "[app].[TeamIdentityDivision]";
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
                Id, TeamIdentityId, DivisionId, StartYear, EndYear
            FROM 
                [app].[TeamIdentityDivision]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, TeamIdentityId, DivisionId, StartYear, EndYear
            FROM 
                [app].[TeamIdentityDivision]
            ORDER BY
                StartYear";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, TeamIdentityId, DivisionId, StartYear, EndYear
            FROM 
                [app].[TeamIdentityDivision]
            WHERE           
                StartYear like @SearchTerms
            ORDER BY
                StartYear";

        private const string _insertSql = @"
            SET NOCOUNT ON;
	        DECLARE @ExistingId	int;
	        SET @ExistingId = NULL;

	        SELECT TOP 1
		        @ExistingId = Id
	        FROM	
		        [app].[TeamIdentityDivision]
	        WHERE	
		        TeamIdentityId = @TeamIdentityId
            AND
                StartYear = @StartYear
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[TeamIdentityDivision]
		        (TeamIdentityId, DivisionId, StartYear, EndYear)
		        VALUES
		        (@TeamIdentityId, @DivisionId, @StartYear, @EndYear)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[TeamIdentityDivision]
	            WHERE	
		            TeamIdentityId = @TeamIdentityId
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
		        [app].[TeamIdentityDivision]
	        WHERE	
		        TeamIdentityId = @TeamIdentityId
            AND
                StartYear = @StartYear

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[TeamIdentityDivision]
                SET
                    TeamIdentityId = @TeamIdentityId,
                    DivisionId = @DivisionId,                    
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
