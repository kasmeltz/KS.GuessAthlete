using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    public interface IAthleteAwardRepository : IDataRepository<AthleteAward>
    {
        /// <summary>
        /// Returns all of the Athlete Awards in the data store for the specified Athlete.
        /// </summary>
        /// <returns>All of the Athlete Awards in the data store for the specified Athlete.</returns>
        Task<IEnumerable<AthleteAward>> ForAthlete(int id);
    }
}
