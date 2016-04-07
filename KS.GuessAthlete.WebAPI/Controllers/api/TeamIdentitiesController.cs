using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    public class TeamIdentitiesController : BaseApiController
    {
        public TeamIdentitiesController() : base () { }

        // GET: api/teamidentities
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                ITeamIdentityRepository repository =
                    RepositoryCollection.TeamIdentities();
                IEnumerable<TeamIdentity> teamIdentities = await repository.List();
                return Request.CreateResponse(HttpStatusCode.OK, teamIdentities);
            }
            catch (Exception ex)
            {
                return CreateResponseError("TEAM_IDENTITY_LIST_ERROR", ex);
            }
        }        
    }
}
