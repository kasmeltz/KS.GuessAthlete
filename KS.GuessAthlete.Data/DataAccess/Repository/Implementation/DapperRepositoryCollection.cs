using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    public class DapperRepositoryCollection : IRepositoryCollection
    {
        protected ICacheProvider CacheProvider { get; set; }

        public DapperRepositoryCollection(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }

        public IAthleteAwardRepository AthleteAwards()
        {
            return new DapperAthleteAwardRepository(CacheProvider);
        }

        public IAthleteRepository Athletes()
        {
            return new DapperAthleteRepository(CacheProvider);
        }

        public IAwardRepository Awards()
        {
            return new DapperAwardRepository(CacheProvider);
        }

        public IDraftRepository Drafts()
        {
            return new DapperDraftRepository(CacheProvider);
        }

        public IGoalieStatLineRepository GoalieStatLines()
        {
            return new DapperGoalieStatLineRepository(CacheProvider);
        }
    }
}
