using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.Data.DataAccess.Exceptions;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
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

        public TeamIdentity GetTeamIdentityFromNamesAndYear(string name, int year, IEnumerable<TeamIdentity> teamIdentities)
        {
            IEnumerable<TeamIdentity> possibleCityIdentities = teamIdentities
                .Where(ti => ti.City.Contains(name) || name.Contains(ti.City));
            IEnumerable<TeamIdentity> possibleNameIdentities = teamIdentities
                .Where(ti => ti.Name.Contains(name) || name.Contains(ti.Name));
            IEnumerable<TeamIdentity> possibleAbbreviationIdentities = teamIdentities
                .Where(ti => ti.Abbreviation.Contains(name) || name.Contains(ti.Abbreviation));

            List<TeamIdentity> possibleIdentities = new List<TeamIdentity>();
            possibleIdentities.AddRange(possibleCityIdentities);
            possibleIdentities.AddRange(possibleNameIdentities);
            possibleIdentities.AddRange(possibleAbbreviationIdentities);

            foreach (TeamIdentity teamIdentity in possibleIdentities)
            {
                if (teamIdentity.StartDate.Year <= year)
                {
                    if (!teamIdentity.EndDate.HasValue)
                    {
                        return teamIdentity;
                    }
                    else
                    {
                        if (teamIdentity.EndDate.Value.Year >= year)
                        {
                            return teamIdentity;
                        }
                    }
                }
            }
            return null;
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

            //Athlete athleteToAdd = scraper.LoadAthlete(@"/players/r/roypa01.html", "G");
            //athleteToAdd.Name = "Patrick Roy";

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
                        .Where(ath => ath.Name == athleteToAdd.Name)
                        .FirstOrDefault();
                }

                foreach (Draft draft in athleteToAdd.Drafts)
                {
                    draft.AthleteId = athlete.Id;

                    TeamIdentity teamIdentity = GetTeamIdentityFromNamesAndYear(draft.TeamName,
                        draft.Year, teamIdentities);

                    if (teamIdentity != null)
                    {
                        draft.TeamIdentityId = teamIdentity.Id;
                        try
                        {
                            await APIClient.Post("api/drafts", draft);
                        }
                        catch (ItemAlreadyExistsException)
                        {
                        }
                    }
                    else
                    {
                        throw new Exception("Team not found for Athlete " + athleteToAdd.Name + " draft for team " + draft.TeamName + " for year " + draft.Year);
                    }
                }

                foreach (JerseyNumber jerseyNumber in athleteToAdd.JerseyNumbers)
                {
                    jerseyNumber.AthleteId = athlete.Id;

                    TeamIdentity teamIdentity = GetTeamIdentityFromNamesAndYear(jerseyNumber.TeamName,
                        jerseyNumber.StartYear, teamIdentities);

                    if (teamIdentity != null)
                    {
                        jerseyNumber.TeamIdentityId = teamIdentity.Id;
                        try
                        {
                            await APIClient.Post("api/jerseyNumbers", jerseyNumber);
                        }
                        catch (ItemAlreadyExistsException)
                        {
                        }
                    }
                    else
                    {
                        throw new Exception("Team not found for Athlete " + athleteToAdd.Name + " jersey number for team " + jerseyNumber.TeamName + " for year " + jerseyNumber.StartYear);
                    }
                }

                foreach (StatLine statLine in athleteToAdd.Stats)
                {
                    statLine.AthleteId = athlete.Id;

                    TeamIdentity teamIdentity = GetTeamIdentityFromNamesAndYear(statLine.TeamName,
                        statLine.Year, teamIdentities);

                    if (teamIdentity != null)
                    {
                        statLine.TeamIdentityId = teamIdentity.Id;

                        Season season = seasons
                            .Where(sea => sea.StartDate.Year == statLine.Year)
                            .FirstOrDefault();

                        if (season != null)
                        {
                            statLine.SeasonId = season.Id;
                            try
                            {
                                GoalieStatLine gsl = statLine as GoalieStatLine;
                                if (gsl != null)
                                {
                                    await APIClient.Post("api/goaliestatlines", gsl);
                                }
                                SkaterStatLine ssl = statLine as SkaterStatLine;
                                if (ssl != null)
                                {
                                    await APIClient.Post("api/skaterstatlines", ssl);
                                }
                            }
                            catch (ItemAlreadyExistsException)
                            {
                            }

                            if (!string.IsNullOrEmpty(statLine.Awards))
                            {
                                string[] awardNames = statLine.Awards.Split(new string[] { "@" },
                                    StringSplitOptions.RemoveEmptyEntries);
                                foreach (string awardName in awardNames)
                                {
                                    string strippedAwardName = awardName.ToLower().Replace("<b>", "").Replace("</b>", "");
                                    string[] awardParts = strippedAwardName.Split('-');

                                    if (awardParts.Length == 2)
                                    {
                                        Award award = awards
                                            .Where(awa => awa.Abbreviation.ToLower() == awardParts[0])
                                            .FirstOrDefault();

                                        if (award != null)
                                        {
                                            AthleteAward athleteAward = new AthleteAward();
                                            athleteAward.AthleteId = athlete.Id;
                                            athleteAward.SeasonId = season.Id;
                                            athleteAward.AwardId = award.Id;
                                            athleteAward.Position = int.Parse(awardParts[1]);

                                            try
                                            {
                                                await APIClient.Post("api/athleteawards", athleteAward);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Season not found " + statLine.Year);
                        }
                    }
                    else
                    {
                        throw new Exception("Team not found for Athlete " + athleteToAdd.Name + " stat line " + statLine.TeamName + " for year " + statLine.Year);
                    }
                }
            }
        }
    }
}
