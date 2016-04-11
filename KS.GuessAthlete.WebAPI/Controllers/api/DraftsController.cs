using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class DraftsController : BaseApiController
    {
        public DraftsController() : base() { }

        // GET: api/drafts/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                IDraftRepository repository =
                    RepositoryCollection.Drafts();
                IEnumerable<Draft> drafts = await repository.ForAthlete(id);
                return Request.CreateResponse(HttpStatusCode.OK, drafts);
            }
            catch (Exception ex)
            {
                return CreateResponseError("DRAFTS_FOR_ATHLETE_ERROR", ex);
            }
        }

        // POST: api/drafts
        public async Task<HttpResponseMessage> Post(Draft draft)
        {
            try
            {
                IDraftRepository repository =
                           RepositoryCollection.Drafts();
                await repository.Insert(draft);
                return Request.CreateResponse(HttpStatusCode.OK, draft);
            }
            catch (Exception ex)
            {
                return CreateResponseError("DRAFT_INSERT_ERROR", ex);
            }
        }        
    }
}
