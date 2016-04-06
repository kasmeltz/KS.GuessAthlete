using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class LeaguesController : BaseApiController
    {
        public LeaguesController() : base () { }

        // GET: api/leagues
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                ILeagueRepository repository =
                    RepositoryCollection.Leagues();
                IEnumerable<League> leagues = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, leagues);
            }
            catch (Exception ex)
            {
                return CreateResponseError("LEAGUE_LIST_ERROR", ex);
            }
        }        
    }
}
