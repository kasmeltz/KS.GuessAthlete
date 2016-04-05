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
        IConferenceRepository Conferences();
        IDivisionRepository Divisions();
        IDraftRepository Drafts();
        IGoalieStatLineRepository GoalieStatLines();
        IJerseyNumberRepository JerseyNumbers();
        ILeagueRepository Leagues();
        ISeasonRepository Seasons();
        ISkaterStatLineRepository SkaterStatLines();
        ITeamIdentityDivisionRepository TeamIdentityDivisions();
        ITeamIdentityRepository TeamIdentities();
        ITeamRepository Teams();        
    }
}
