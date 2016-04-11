using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class JerseyNumbersController : BaseApiController
    {
        public JerseyNumbersController() : base() { }

        // GET: api/jerseynumbers/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                IJerseyNumberRepository repository =
                    RepositoryCollection.JerseyNumbers();
                IEnumerable<JerseyNumber> jerseyNumbers = await repository.ForAthlete(id);
                return Request.CreateResponse(HttpStatusCode.OK, jerseyNumbers);
            }
            catch (Exception ex)
            {
                return CreateResponseError("JERSEY_NUMBERS_FOR_ATHLETE_ERROR", ex);
            }
        }

        // POST: api/jerseynumbers
        public async Task<HttpResponseMessage> Post(JerseyNumber jerseyNumber)
        {
            try
            {
                IJerseyNumberRepository repository =
                           RepositoryCollection.JerseyNumbers();
                await repository.Insert(jerseyNumber);
                return Request.CreateResponse(HttpStatusCode.OK, jerseyNumber);
            }
            catch (Exception ex)
            {
                return CreateResponseError("JERSEY_NUMBER_INSERT_ERROR", ex);
            }
        }        
    }
}
