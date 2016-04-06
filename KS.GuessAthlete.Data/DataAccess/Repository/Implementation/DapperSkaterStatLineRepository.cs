using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO.Hockey;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to SkaterStatLine data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperSkaterStatLineRepository : BaseDapperRepository<SkaterStatLine>,
        ISkaterStatLineRepository
    {        
        public DapperSkaterStatLineRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "SkaterStatLine";
            TableName = "[app].[SkaterStatLine]";
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
                Id, AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, Goals, Assists, PlusMinus, PenaltyMinutes, 
                EvenStrengthGoals, PowerPlayGoals, ShortHandedGoals, GameWinningGoals,
                EvenStrengthAssists, PowerPlayAssists, ShortHandedAssists, 
                Shots, ShotPercentage, TimeOnIce, AverageTimeOnIce,
                StanleyCup, IsPlayoffs

            FROM 
                [app].[SkaterStatLine]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, Goals, Assists, PlusMinus, PenaltyMinutes, 
                EvenStrengthGoals, PowerPlayGoals, ShortHandedGoals, GameWinningGoals,
                EvenStrengthAssists, PowerPlayAssists, ShortHandedAssists, 
                Shots, ShotPercentage, TimeOnIce, AverageTimeOnIce,
                StanleyCup, IsPlayoffs
            FROM 
                [app].[SkaterStatLine]
            ORDER BY
                AthleteId";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, Goals, Assists, PlusMinus, PenaltyMinutes, 
                EvenStrengthGoals, PowerPlayGoals, ShortHandedGoals, GameWinningGoals,
                EvenStrengthAssists, PowerPlayAssists, ShortHandedAssists, 
                Shots, ShotPercentage, TimeOnIce, AverageTimeOnIce,
                StanleyCup, IsPlayoffs
            FROM 
                [app].[SkaterStatLine]
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
		        [app].[SkaterStatLine]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                TeamIdentityId = @TeamIdentityId
            AND
                SeasonId = @SeasonId

	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[SkaterStatLine]
		        (AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, Goals, Assists, PlusMinus, PenaltyMinutes, 
                EvenStrengthGoals, PowerPlayGoals, ShortHandedGoals, GameWinningGoals,
                EvenStrengthAssists, PowerPlayAssists, ShortHandedAssists, 
                Shots, ShotPercentage, TimeOnIce, AverageTimeOnIce,
                StanleyCup, IsPlayoffs)
		        VALUES
		        (@AthleteId, @TeamIdentityId, @SeasonId, 
                @GamesPlayed, @Goals, @Assists, @PlusMinus, @PenaltyMinutes, 
                @EvenStrengthGoals, @PowerPlayGoals, @ShortHandedGoals, @GameWinningGoals,
                @EvenStrengthAssists, @PowerPlayAssists, @ShortHandedAssists, 
                @Shots, @ShotPercentage, @TimeOnIce, @AverageTimeOnIce,
                @StanleyCup, @IsPlayoffs)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[SkaterStatLine]
	            WHERE	
		            AthleteId = @AthleteId
                AND
                    TeamIdentityId = @TeamIdentityId
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
		        [app].[SkaterStatLine]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                TeamIdentityId = @TeamIdentityId
            AND
                SeasonId = @SeasonId

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[SkaterStatLine]
                SET
                    AthleteId = @AthleteId,
                    TeamIdentityId = @TeamIdentityId,
                    SeasonId = @SeasonId,
                    GamesPlayed = @GamesPlayed,
                    Goals = @Goals,
                    Assists = @Assists,
                    PlusMinus = @PlusMinus,
                    PenaltyMinutes = @PenaltyMinutes,
                    EvenStrengthGoals = @EvenStrengthGoals,
                    PowerPlayGoals = @PowerPlayGoals,
                    ShortHandedGoals = @ShortHandedGoals,
                    GameWinningGoals = @GameWinningGoals,
                    EvenStrengthAssists = @EvenStrengthAssists,
                    PowerPlayAssists = @PowerPlayAssists,
                    ShortHandedAssists = @ShortHandedAssists,
                    Shots = @Shots,
                    ShotPercentage = @ShotPercentage,
                    TimeOnIce = @TimeOnIce,
                    AverageTimeOnIce = @AverageTimeOnIce,
                    StanleyCup = @StanleyCup,
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
