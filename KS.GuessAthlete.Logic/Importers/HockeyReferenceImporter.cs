using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.Data.DataAccess.Exceptions;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Logic.Scrapers.Hockey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Logic.Importers
{
    public class HockeyReferenceImporter
    {
        protected IRestfulClient APIClient { get; set; }

        public HockeyReferenceImporter(IRestfulClient apiClient)
        {
            APIClient = apiClient;
        }

        public async Task ImportAthletes()
        {
            IEnumerable<Athlete> athletes = await APIClient
                .Get<IEnumerable<Athlete>>("api/athletes");

            IEnumerable<Season> seasons = await APIClient
                .Get<IEnumerable<Season>>("api/seasons");

            IEnumerable<Award> awards = await APIClient
                .Get<IEnumerable<Award>>("api/awards");

            IEnumerable<TeamIdentity> teamIdentities = await APIClient
               .Get<IEnumerable<TeamIdentity>>("api/teamidentities");

            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            //IEnumerable<Athlete> scrapedAthlete = scraper.ScrapeAthleteData(athletes);
            IEnumerable<Athlete> scrapedAthlete = scraper.ScrapeAthleteData();
            foreach (Athlete athleteToAdd in scrapedAthlete)
            {
                Athlete athlete = null;

                try
                {
                    athlete = await APIClient.Post("api/athletes", athleteToAdd);
                }
                catch (Exception)
                {
                    athlete = athletes
                        .Where(ath => ath.Name == athlete.Name)
                        .FirstOrDefault();
                }

                foreach (Draft draft in athlete.Drafts)
                {
                    draft.AthleteId = athlete.Id;

                    TeamIdentity teamIdentity = teamIdentities
                        .Where(ti => ti.City == draft.TeamName)
                        .FirstOrDefault();

                    if (teamIdentity != null)
                    {
                        draft.TeamIdentityId = teamIdentity.Id;
                        try
                        {
                            //await draftRepository.Insert(draft);
                        }
                        catch (ItemAlreadyExistsException)
                        {
                        }
                    }
                }

                foreach(JerseyNumber jerseyNumber in athlete.JerseyNumbers)
                {
                    jerseyNumber.AthleteId = athlete.Id;

                    TeamIdentity teamIdentity = teamIdentities
                        .Where(ti => ti.Name == jerseyNumber.TeamName)
                        .FirstOrDefault();

                    if (teamIdentity != null)
                    {
                        jerseyNumber.TeamIdentityId = teamIdentity.Id;
                        try
                        {
                            //await jerseyNumberRepository.Insert(draft);
                        }
                        catch (ItemAlreadyExistsException)
                        {
                        }
                    }
                }

                foreach(StatLine statLine in athlete.Stats)
                {
                    statLine.AthleteId = athlete.Id;

                    TeamIdentity teamIdentity = teamIdentities
                        .Where(ti => ti.Name == statLine.TeamName)
                        .FirstOrDefault();

                    if (teamIdentity != null)
                    {
                        statLine.TeamIdentityId = teamIdentity.Id;                                            
                    }                     
                }

                /*
                 public IEnumerable<StatLine> Stats { get; set; }
        public IEnumerable<Draft> Drafts { get; set; }
        public IEnumerable<JerseyNumber> JerseyNumbers { get; set; }
        public IEnumerable<AthleteAward> Awards { get; set; }
        */

            }
        }
    }
}
