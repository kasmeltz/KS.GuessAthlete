using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    public interface IAthleteRepository : IDataRepository<Athlete>
    {
        /// <summary>
        /// Returns all of the Skater Athletes in the data store that exceed the specified criteria.
        /// </summary>
        /// <returns>All of the Skater Athletes in the data store that exceed the specified criteria.</returns>
        Task<IEnumerable<int>> SkatersForCriteria(int gamesPlayed, int points, decimal ppg, int startYear);
    }
}
