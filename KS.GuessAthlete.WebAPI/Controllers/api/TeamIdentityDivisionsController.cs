using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class TeamIdentityDivisionsController : BaseApiController
    {
        public TeamIdentityDivisionsController() : base() { }

        // GET: api/teamidentitydivisions
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                ITeamIdentityDivisionRepository repository =
                    RepositoryCollection.TeamIdentityDivisions();
                IEnumerable<TeamIdentityDivision> teamIdentityDivisions = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, teamIdentityDivisions);
            }
            catch (Exception ex)
            {
                return CreateResponseError("TEAM_IDENTITY_DIVISION_LIST", ex);
            }
        }
    }
}
