using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to Athlete Award data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperAthleteAwardRepository : BaseDapperRepository<AthleteAward>,
        IAthleteAwardRepository
    {        
        public DapperAthleteAwardRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "AthleteAward";
            TableName = "[app].[AthleteAward]";
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
                Id, AwardId, AthleteId, SeasonId
            FROM 
                [app].[AthleteAward]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AwardId, AthleteId, SeasonId
            FROM 
                [app].[AthleteAward]
            ORDER BY
                AwardId, AthleteId";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AwardId, AthleteId, SeasonId
            FROM 
                [app].[AthleteAward]
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
		        [app].[AthleteAward]
	        WHERE	
		        AwardId = @AwardId
            AND
                AthleteId = @AthleteId
            AND 
                SeasonId = @SeasonId
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[AthleteAward]
		        (AwardId, AthleteId, SeasonId)
		        VALUES
		        (@AwardId, @AthleteId, @SeasonId)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[AthleteAward]
	            WHERE	
		            AwardId = @AwardId
                AND
                    AthleteId = @AthleteId
                AND 
                    SeasonId = @SeasonId 
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
		        [app].[AthleteAward]
	        WHERE	
		        AwardId = @AwardId
            AND
                AthleteId = @AthleteId
            AND 
                SeasonId = @SeasonId

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[AthleteAward]
                SET
                    AwardId = @AwardId,
                    AthleteId = @AthleteId,                    
                    SeasonId = @SeasonId
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
