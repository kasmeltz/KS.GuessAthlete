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

        public IAthleteRepository Athletes()
        {
            return new DapperAthleteRepository(CacheProvider);
        }
    }
}
