using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to City data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperAthleteRepository : BaseDapperRepository<Athlete>,
        IAthleteRepository
    {        
        public DapperAthleteRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            BulkColumnMapping = _bulkColumnMapping;
            StagingTableName = "CityImportStaging";
            CacheContainerName = "City";
            TableName = "[mlist].[City]";
            CacheSeconds = 3600;
        }

        protected override void CreateSql()
        {
            FinalizeBulkImportSQL = _finalizeBulkImportSql;
            GetSql = _getSql;
            ListSql = _listSql;
            SearchSql = _searchSql;
            InsertSql = _insertSql;
            UpdateSql = _updateSql;
        }

        private const string _finalizeBulkImportSql = @"
            SET NOCOUNT ON;
            MERGE [mlist].[City] AS T
            USING [dbo].[CityImportStaging] AS S
            ON (T.StateId = S.StateId AND T.Name = S.Name)
            WHEN NOT MATCHED BY TARGET THEN 
                INSERT
                    (StateId, Name, Abbreviation)
                VALUES
                    (S.StateId, S.Name, S.Abbreviation)
            WHEN MATCHED THEN 
                UPDATE 
                    SET 
                    T.StateId = S.StateId,
                    T.Name = S.Name, 
                    T.Abbreviation = S.Abbreviation;

            DELETE FROM [dbo].[CityImportStaging]";

        private const string _getSql = @"
            SET NOCOUNT ON;
            SELECT TOP 1
                Id, StateId, Name, Abbreviation
            FROM 
                [mlist].[City]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, StateId, Name, Abbreviation
            FROM 
                [mlist].[City]
            ORDER BY
                Name, Abbreviation";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, StateId, Name, Abbreviation
            FROM 
                [mlist].[City]
            WHERE           
                Name like @SearchTerms
            ORDER BY
                Name, Abbreviation";

        private const string _insertSql = @"
            SET NOCOUNT ON;
	        DECLARE @ExistingId	int;
	        SET @ExistingId = NULL;

	        SELECT TOP 1
		        @ExistingId = Id
	        FROM	
		        [mlist].[City]
	        WHERE	
		        Name = @Name
            AND
                StateId = @StateId
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [mlist].[City]
		        (StateId, Name, Abbreviation)
		        VALUES
		        (@StateId, @Name, @Abbreviation)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [mlist].[City]
	            WHERE	
		            Name = @Name
                AND
                    StateId = @StateId            
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
		        [mlist].[City]
	        WHERE	
		        Name = @Name
            AND
                StateId = @StateId

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [mlist].[City]
                SET
                    StateId = @StateId,
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
