using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Logic.Importers
{
    public class HockeyReferenceImporter
    {
        protected IRestfulClient APIClient { get; set; }
        protected IRepositoryCollection Repository { get; set; }

        public HockeyReferenceImporter(IRestfulClient apiClient, IRepositoryCollection repository)
        {
            APIClient = apiClient;
            Repository = repository;
        }

        public async Task ImportAthletes()
        {
            IEnumerable<Athlete> athletes = await APIClient.GetBatch<Athlete>(500, "api/athletes"); 
        }
    }
}
