using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class SkaterStatLinesController : BaseApiController
    {
        public SkaterStatLinesController() : base() { }

        // POST: api/skatersstatlines
        public async Task<HttpResponseMessage> Post(SkaterStatLine skaterStatLine)
        {
            try
            {
                ISkaterStatLineRepository repository =
                           RepositoryCollection.SkaterStatLines();
                await repository.Insert(skaterStatLine);
                return Request.CreateResponse(HttpStatusCode.OK, skaterStatLine);
            }
            catch (Exception ex)
            {
                return CreateResponseError("SKATER_STAT_LINES_INSERT_ERROR", ex);
            }
        }        
    }
}
