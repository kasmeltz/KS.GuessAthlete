using KS.GuessAthlete.Component.Caching.Implementation;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Component.Logging.Implemetation;
using KS.GuessAthlete.Component.Logging.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Implementation;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace KS.GuessAthlete.WebAPI.Controllers.api
{
    /// <summary>
    /// Contains the logic used across all of the api controllers
    /// </summary>
    [Authorize]
    public class BaseApiController : ApiController
    {
        protected ICacheProvider CacheProvider { get; set; }
        protected IRepositoryCollection RepositoryCollection { get; set; }
        protected ILogger Logger { get; set; }

        public BaseApiController()
        {
            CacheProvider = MemoryCacheProvider.Instance;
            Logger = EnterpriseLogger.Instance;
            RepositoryCollection = new DapperRepositoryCollection(CacheProvider);
        }

        public BaseApiController(ICacheProvider cacheProvider, ILogger logger, IRepositoryCollection repositoryCollection)
        {
            CacheProvider = cacheProvider;
            Logger = logger;
            RepositoryCollection = repositoryCollection;
        }

        protected string AuthenticatedUserId
        {
            get
            {
                if (User != null)
                {
                    IIdentity identity = User.Identity;
                    if (identity != null)
                    {
                        return identity.GetUserId();
                    }
                }

                return string.Empty;
            }
        }

        /*
        protected UserProfile _authenticatedUserProfile;
        protected async Task<UserProfile> AuthenticatedUserProfile()
        {
            if (_authenticatedUserProfile == null)
            {
                IUserProfileRepository repository =
                    RepositoryCollection.UserProfiles();

                _authenticatedUserProfile = await repository
                    .GetByUserId(AuthenticatedUserId);
            }

            return _authenticatedUserProfile;
        }
        */

        public HttpResponseMessage CreateAccessError(string errorMessage)
        {
            errorMessage = string.Format(
                "{0}: UserId: {1} URL: {2}",
                errorMessage,
                AuthenticatedUserId,
                Request.RequestUri);

            Logger.Error(this, errorMessage);
            HttpError err = new HttpError(errorMessage);
            return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
        }

        public HttpResponseMessage CreateResponseError(string errorMessage)
        {
            Logger.Error(this, errorMessage);
            HttpError err = new HttpError(errorMessage);
            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        }

        public HttpResponseMessage CreateResponseError(string errorMessage, Exception ex)
        {
            Logger.Error(this, errorMessage, ex);
            HttpError err = new HttpError(errorMessage);
            return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
        }
    }
}
