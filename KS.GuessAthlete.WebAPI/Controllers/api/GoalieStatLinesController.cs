using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO.Hockey;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class GoalieStatLinesController : BaseApiController
    {
        public GoalieStatLinesController() : base() { }

        // GET: api/goalieStatLines/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                IGoalieStatLineRepository repository =
                    RepositoryCollection.GoalieStatLines();
                IEnumerable<GoalieStatLine> goalieStatLines = await repository.ForAthlete(id);
                return Request.CreateResponse(HttpStatusCode.OK, goalieStatLines);
            }
            catch (Exception ex)
            {
                return CreateResponseError("GOALIE_STAT_LINES_FOR_ATHLETE_ERROR", ex);
            }
        }

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
