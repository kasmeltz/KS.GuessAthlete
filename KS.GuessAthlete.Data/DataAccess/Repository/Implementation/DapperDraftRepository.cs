using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Draft data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperDraftRepository : BaseDapperRepository<Draft>,
        IDraftRepository
    {        
        public DapperDraftRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "Draft";
            TableName = "[app].[Draft]";
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
                Id, AthleteId, TeamIdentityId, Year, Round, Position
            FROM 
                [app].[Draft]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, Year, Round, Position
            FROM 
                [app].[Draft]
            ORDER BY
                AthleteId";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, Year, Round, Position
            FROM 
                [app].[Draft]
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
		        [app].[Draft]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                Year = @Year
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[Draft]
		        (AthleteId, TeamIdentityId, Year, Round, Position)
		        VALUES
		        (@AthleteId, @TeamIdentityId, @Year, @Round, @Position)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[Draft]
	            WHERE	
		            AthleteId = @AthleteId
                AND
                    Year = @Year 
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
		        [app].[Draft]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                Year = @Year

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[Draft]
                SET
                    AthleteId = @AthleteId,
                    TeamIdentityId = @TeamIdentityId,
                    Year = @Year,
                    Round = @Round,
                    Position = @Position
		        WHERE	
		            Id = @Id
                    
                SELECT @Id
            END
            ELSE
            BEGIN
                SELECT -1
            END";

        private const string _forAthleteSql = @"
             SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, Year, Round, Position
            FROM 
                [app].[Draft]
            WHERE
                AthleteId = @Id";

        public async Task<IEnumerable<Draft>> ForAthlete(int id)
        {
            return await List(_forAthleteSql, new
            {
                Id = id
            }, "DraftsForAthlete" + id);
        }
    }
}
