using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    public interface IJerseyNumberRepository : IDataRepository<JerseyNumber>
    {
        /// <summary>
        /// Returns all of the Jersey Numbers in the data store for the specified Athlete.
        /// </summary>
        /// <returns>All of the Jersey Numbers in the data store for the specified Athlete.</returns>
        Task<IEnumerable<JerseyNumber>> ForAthlete(int id);
    }
}
