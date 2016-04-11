using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    public interface IDraftRepository : IDataRepository<Draft>
    {
        /// <summary>
        /// Returns all of the Drafts in the data store for the specified Athlete.
        /// </summary>
        /// <returns>All of the Drafts in the data store for the specified Athlete.</returns>
        Task<IEnumerable<Draft>> ForAthlete(int id);
    }
}
