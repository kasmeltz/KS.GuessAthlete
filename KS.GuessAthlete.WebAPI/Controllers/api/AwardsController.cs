using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class AwardsController : BaseApiController
    {
        public AwardsController() : base () { }

        // GET: api/awards
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                IAwardRepository repository =
                    RepositoryCollection.Awards();
                IEnumerable<Award> awards = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, awards);
            }
            catch (Exception ex)
            {
                return CreateResponseError("AWARD_LIST_ERROR", ex);
            }
        }        
    }
}
