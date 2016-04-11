using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class AthleteAwardsController : BaseApiController
    {
        public AthleteAwardsController() : base() { }

        // GET: api/athleteAwards/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                IAthleteAwardRepository repository =
                    RepositoryCollection.AthleteAwards();
                IEnumerable<AthleteAward> athleteAwards = await repository.ForAthlete(id);
                return Request.CreateResponse(HttpStatusCode.OK, athleteAwards);
            }
            catch (Exception ex)
            {
                return CreateResponseError("ATHLETE_AWARDS_FOR_ATHLETE_ERROR", ex);
            }
        }

        // POST: api/athleteAwards
        public async Task<HttpResponseMessage> Post(AthleteAward athleteAward)
        {
            try
            {
                IAthleteAwardRepository repository =
                           RepositoryCollection.AthleteAwards();
                await repository.Insert(athleteAward);
                return Request.CreateResponse(HttpStatusCode.OK, athleteAward);
            }
            catch (Exception ex)
            {
                return CreateResponseError("ATHLETE_AWARD_INSERT_ERROR", ex);
            }
        }        
    }
}
