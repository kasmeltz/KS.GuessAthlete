using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class SeasonsController : BaseApiController
    {
        public SeasonsController() : base () { }

        // GET: api/seasons
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                ISeasonRepository repository =
                    RepositoryCollection.Seasons();
                IEnumerable<Season> seasons = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, seasons);
            }
            catch (Exception ex)
            {
                return CreateResponseError("SEASON_LIST_ERROR", ex);
            }
        }        
    }
}
