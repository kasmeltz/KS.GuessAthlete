using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class DivisionsController : BaseApiController
    {
        public DivisionsController() : base () { }

        // GET: api/divisions
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                IDivisionRepository repository =
                    RepositoryCollection.Divisions();
                IEnumerable<Division> divisions = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, divisions);
            }
            catch (Exception ex)
            {
                return CreateResponseError("DIVISION_LIST_ERROR", ex);
            }
        }        
    }
}
