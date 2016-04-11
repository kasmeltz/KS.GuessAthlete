using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO.Hockey;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Provides access to GoalieStatLine data in the data store using the Dapper framework.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DapperGoalieStatLineRepository : BaseDapperRepository<GoalieStatLine>,
        IGoalieStatLineRepository
    {        
        public DapperGoalieStatLineRepository(ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            CacheContainerName = "GoalieStatLine";
            TableName = "[app].[GoalieStatLine]";
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
                GamesPlayed, GamesStarted, 
                Wins, Losses, TiesPlusOvertimeShootoutLosses, 
                GoalsAgainst, ShotsAgainst, Saves, 
                SavePercentage, GoalsAgainstAverage,
                Shutouts, Minutes,
                QualityStarts, QualityStartPercentage, ReallyBadStarts, 
                GoalsAgainstPercentage, GoalsSavedAboveAverage, GoaliePointShares,
                Goals, Assists, PenaltyMinutes, StanleyCup, IsPlayoffs

            FROM 
                [app].[GoalieStatLine]
            WHERE
                Id = @Id";

        private const string _listSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, GamesStarted, 
                Wins, Losses, TiesPlusOvertimeShootoutLosses, 
                GoalsAgainst, ShotsAgainst, Saves, 
                SavePercentage, GoalsAgainstAverage,
                Shutouts, Minutes,
                QualityStarts, QualityStartPercentage, ReallyBadStarts, 
                GoalsAgainstPercentage, GoalsSavedAboveAverage, GoaliePointShares,
                Goals, Assists, PenaltyMinutes, StanleyCup, IsPlayoffs
            FROM 
                [app].[GoalieStatLine]
            ORDER BY
                AthleteId";

        private const string _searchSql = @"
            SET NOCOUNT ON;
            SELECT  
                Id, AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, GamesStarted, 
                Wins, Losses, TiesPlusOvertimeShootoutLosses, 
                GoalsAgainst, ShotsAgainst, Saves, 
                SavePercentage, GoalsAgainstAverage,
                Shutouts, Minutes,
                QualityStarts, QualityStartPercentage, ReallyBadStarts, 
                GoalsAgainstPercentage, GoalsSavedAboveAverage, GoaliePointShares,
                Goals, Assists, PenaltyMinutes, StanleyCup, IsPlayoffs
            FROM 
                [app].[GoalieStatLine]
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
		        [app].[GoalieStatLine]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                TeamIdentityId = @TeamIdentityId
            AND
                SeasonId = @SeasonId
            AND
                IsPlayoffs = @IsPlayoffs
	            
	        IF(@ExistingId IS NULL)
	        BEGIN
		        INSERT INTO [app].[GoalieStatLine]
		        (AthleteId, TeamIdentityId, SeasonId, 
                GamesPlayed, GamesStarted, 
                Wins, Losses, TiesPlusOvertimeShootoutLosses, 
                GoalsAgainst, ShotsAgainst, Saves, 
                SavePercentage, GoalsAgainstAverage,
                Shutouts, Minutes,
                QualityStarts, QualityStartPercentage, ReallyBadStarts, 
                GoalsAgainstPercentage, GoalsSavedAboveAverage, GoaliePointShares,
                Goals, Assists, PenaltyMinutes, StanleyCup, IsPlayoffs)
		        VALUES
		        (@AthleteId, @TeamIdentityId, @SeasonId, 
                @GamesPlayed, @GamesStarted, 
                @Wins, @Losses, @TiesPlusOvertimeShootoutLosses, 
                @GoalsAgainst, @ShotsAgainst, @Saves, 
                @SavePercentage, @GoalsAgainstAverage,
                @Shutouts, @Minutes,
                @QualityStarts, @QualityStartPercentage, @ReallyBadStarts, 
                @GoalsAgainstPercentage, @GoalsSavedAboveAverage, @GoaliePointShares,
                @Goals, @Assists, @PenaltyMinutes, @StanleyCup, @IsPlayoffs)

		        SELECT TOP 1 
			        Id
		        FROM	
		            [app].[GoalieStatLine]
	            WHERE	
		            AthleteId = @AthleteId
                AND
                    TeamIdentityId = @TeamIdentityId
                AND
                    SeasonId = @SeasonId    
                AND
                    IsPlayoffs = @IsPlayoffs
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
		        [app].[GoalieStatLine]
	        WHERE	
		        AthleteId = @AthleteId
            AND
                TeamIdentityId = @TeamIdentityId
            AND
                SeasonId = @SeasonId
            AND
                IsPlayoffs = @IsPlayoffs

            IF(@ExistingId IS NULL OR @ExistingId = @Id)
	        BEGIN
		        UPDATE 
                    [app].[GoalieStatLine]
                SET
                    AthleteId = @AthleteId,
                    TeamIdentityId = @TeamIdentityId,
                    SeasonId = @SeasonId,
                    GamesPlayed = @GamesPlayed,
                    GamesStarted = @GamesStarted,
                    Wins = @Wins,
                    Losses = @Losses,
                    TiesPlusOvertimeShootoutLosses = @TiesPlusOvertimeShootoutLosses,
                    GoalsAgainst = @GoalsAgainst,
                    ShotsAgainst = @ShotsAgainst,
                    Saves = @Saves,
                    SavePercentage = @SavePercentage,
                    GoalsAgainstAverage = @GoalsAgainstAverage,
                    Shutouts = @Shutouts,
                    Minutes = @Minutes,
                    QualityStarts = @QualityStarts,
                    QualityStartPercentage = @QualityStartPercentage,
                    ReallyBadStarts = @ReallyBadStarts,
                    GoalsAgainstPercentage = @GoalsAgainstPercentage,
                    GoalsSavedAboveAverage = @GoalsSavedAboveAverage,
                    GoaliePointShares = @GoaliePointShares,
                    Goals = @Goals,
                    Assists = @Assists,
                    PenaltyMinutes = @PenaltyMinutes,
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
