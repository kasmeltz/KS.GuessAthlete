using KS.GuessAthlete.Data.POCO.Hockey;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Interface
{
    public interface IGoalieStatLineRepository : IDataRepository<GoalieStatLine>
    {
        /// <summary>
        /// Returns all of the Goalie Stat Lines in the data store for the specified Athlete.
        /// </summary>
        /// <returns>All of the Goalie Stat Lines in the data store for the specified Athlete.</returns>
        Task<IEnumerable<GoalieStatLine>> ForAthlete(int id);
    }
}
