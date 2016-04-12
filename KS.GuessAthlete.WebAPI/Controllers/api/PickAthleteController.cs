using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Logic.AthletePicker.Hockey;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class PickAthleteController : BaseApiController
    {
        public PickAthleteController() : base() { }

        // GET: api/pickathlete
        public async Task<HttpResponseMessage> Get([FromUri]int skaterGamesPlayed,
            [FromUri]int skaterPoints, [FromUri]decimal skaterPPG, 
            [FromUri]int goalieGamesPlayed, [FromUri]int goalieWins,
            [FromUri]int startYear)
        {
            try
            {
                HockeyAthletePicker picker = new HockeyAthletePicker(RepositoryCollection);
                Athlete athlete = await picker.PickAthlete(skaterGamesPlayed, skaterPoints, skaterPPG, 
                    goalieGamesPlayed, goalieWins, startYear);
                return Request.CreateResponse(HttpStatusCode.OK, athlete);
            }
            catch (Exception ex)
            {
                return CreateResponseError("PICK_ATHLETE_ERROR", ex);
            }
        }
    }
}
