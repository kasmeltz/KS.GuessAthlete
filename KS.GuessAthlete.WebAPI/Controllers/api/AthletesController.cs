using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class AthletesController : BaseApiController
    {
        public AthletesController() : base() { }

        // GET: api/athletes
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                IAthleteRepository repository =
                    RepositoryCollection.Athletes();
                IEnumerable<Athlete> athletes = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, athletes);
            }
            catch (Exception ex)
            {
                return CreateResponseError("ATHLETE_LIST_ERROR", ex);
            }
        }

        // POST: api/athletes
        public async Task<HttpResponseMessage> Post(Athlete athlete)
        {
            try
            {
                IAthleteRepository repository =
                           RepositoryCollection.Athletes();
                await repository.Insert(athlete);
                return Request.CreateResponse(HttpStatusCode.OK, athlete);
            }
            catch (Exception ex)
            {
                return CreateResponseError("ATHLETE_INSERT_ERROR", ex);
            }
        }

        // PUT: api/athletes
        public async Task<HttpResponseMessage> Put(Athlete athlete)
        {
            try
            {
                IAthleteRepository repository =
                          RepositoryCollection.Athletes();
                await repository.Update(athlete);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CreateResponseError("ATHLETE_UDPATE_ERROR", ex);
            }
        }

        // DELETE: api/athletes/id
        public async Task<HttpResponseMessage> Delete(int id)
        {

            try
            {
                IAthleteRepository repository =
                          RepositoryCollection.Athletes();
                await repository.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CreateResponseError("ATHLETE_DELETE_ERROR", ex);
            }
        }
    }
}
