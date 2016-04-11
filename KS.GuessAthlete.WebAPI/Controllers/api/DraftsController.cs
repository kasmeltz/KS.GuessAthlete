using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class DraftsController : BaseApiController
    {
        public DraftsController() : base() { }

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
