using KS.GuessAthlete.Data.POCO.Hockey;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    public interface ISkaterStatLineRepository : IDataRepository<SkaterStatLine>
    {
        /// <summary>
        /// Returns all of the Skater Stat Lines in the data store for the specified Athlete.
        /// </summary>
        /// <returns>All of the Skater Stat Lines in the data store for the specified Athlete.</returns>
        Task<IEnumerable<SkaterStatLine>> ForAthlete(int id);
    }
}
