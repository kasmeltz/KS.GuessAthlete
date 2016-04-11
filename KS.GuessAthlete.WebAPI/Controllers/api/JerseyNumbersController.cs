using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class JerseyNumbersController : BaseApiController
    {
        public JerseyNumbersController() : base() { }

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
