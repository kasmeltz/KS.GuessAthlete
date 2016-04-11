using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class GoalieStatLinesController : BaseApiController
    {
        public GoalieStatLinesController() : base() { }

        // POST: api/goaliesstatlines
        public async Task<HttpResponseMessage> Post(GoalieStatLine goalieStatLine)
        {
            try
            {
                IGoalieStatLineRepository repository =
                           RepositoryCollection.GoalieStatLines();
                await repository.Insert(goalieStatLine);
                return Request.CreateResponse(HttpStatusCode.OK, goalieStatLine);
            }
            catch (Exception ex)
            {
                return CreateResponseError("GOALIE_STAT_LINES_INSERT_ERROR", ex);
            }
        }        
    }
}
