namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    /// <summary>
    /// Represents the collection of repositories used in the Sales Tracker app.
    /// </summary>
    public interface IRepositoryCollection
    {
        IAthleteAwardRepository AthleteAwards();
        IAthleteRepository Athletes();
        IAwardRepository Awards();
        IDraftRepository Drafts();
        IGoalieStatLineRepository GoalieStatLines();
        IJerseyNumberRepository JerseyNumbers();
        ILeagueRepository Leagues();
        ISeasonRepository Seasons();
    }
}
