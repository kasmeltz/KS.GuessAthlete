using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class ConferencesController : BaseApiController
    {
        public ConferencesController() : base () { }

        // GET: api/conferences
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                IConferenceRepository repository =
                    RepositoryCollection.Conferences();
                IEnumerable<Conference> conferences = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, conferences);
            }
            catch (Exception ex)
            {
                return CreateResponseError("CONFERENCE_LIST_ERROR", ex);
            }
        }        
    }
}
