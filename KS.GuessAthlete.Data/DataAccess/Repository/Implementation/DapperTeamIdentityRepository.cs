using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to TeamIdentity data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperTeamIdentityRepository : BaseDapperRepository<TeamIdentity>,
        ITeamIdentityRepository
    {        
        public DapperTeamIdentityRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "TeamIdentity";
            TableName = "[app].[TeamIdentity]";
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
                Id, TeamId, Name, Abbreviation, City, StartDate, EndDate
            FROM 
                [app].[TeamIdentity]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, TeamId, Name, Abbreviation, City, StartDate, EndDate
            FROM 
                [app].[TeamIdentity]
            ORDER BY
                Name";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, TeamId, Name, Abbreviation, City, StartDate, EndDate
            FROM 
                [app].[TeamIdentity]
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
		        [app].[TeamIdentity]
	        WHERE	
		        Name = @Name
            AND
                StartDate = @StartDate
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[TeamIdentity]
		        (TeamId, Name, Abbreviation, City, StartDate, EndDate)
		        VALUES
		        (@TeamId, @Name, @Abbreviation, @City, @StartDate, @EndDate)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[TeamIdentity]
	            WHERE	
		            Name = @Name
                AND
                    StartDate = @StartDate
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
		        [app].[TeamIdentity]
	        WHERE	
		        Name = @Name
            AND
                StartDate = @StartDate

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[TeamIdentity]
                SET
                    TeamId = @TeamId,
                    Name = @Name,
                    Abbreviation = @Abbreviation,
                    City = @City,
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
