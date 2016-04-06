using KS.GuessAthlete.Component.Caching.Implementation;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Implementation;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    public class RepositoryTestHelper
    {
        public static IRepositoryCollection Collection()
        {
            ICacheProvider cacheProvider = new NoOpCacheProvider();
            IRepositoryCollection collection =
                new DapperRepositoryCollection(cacheProvider);
            return collection;
        }


        /// <summary>
        /// Prepares for a data acccess test by starting with a completely empty database.
        /// </summary>
        public static void PrepareForTest()
        {
            IRepositoryCollection collection = Collection();

            IAthleteAwardRepository AthleteAwards = collection.AthleteAwards();
            IAthleteRepository Athletes = collection.Athletes();
            IAwardRepository Awards = collection.Awards();
            IConferenceRepository Conferences = collection.Conferences();
            IDivisionRepository Divisions = collection.Divisions();
            IDraftRepository Drafts = collection.Drafts();
            IGoalieStatLineRepository GoalieStatLines = collection.GoalieStatLines();
            IJerseyNumberRepository JerseyNumbers = collection.JerseyNumbers();
            ILeagueRepository Leagues = collection.Leagues();
            ISeasonRepository Seasons = collection.Seasons();
            ISkaterStatLineRepository SkaterStatLines = collection.SkaterStatLines();
            ITeamIdentityDivisionRepository TeamIdentityDivisions = collection.TeamIdentityDivisions();
            ITeamIdentityRepository TeamIdentities = collection.TeamIdentities();
            ITeamRepository Teams = collection.Teams();

            TeamIdentityDivisions.PurgeForTest().Wait();
            Divisions.PurgeForTest().Wait();
            Conferences.PurgeForTest().Wait();
            GoalieStatLines.PurgeForTest().Wait();
            SkaterStatLines.PurgeForTest().Wait();
            AthleteAwards.PurgeForTest().Wait();
            Drafts.PurgeForTest().Wait();
            JerseyNumbers.PurgeForTest().Wait();
            Awards.PurgeForTest().Wait();
            Athletes.PurgeForTest().Wait();
            TeamIdentities.PurgeForTest().Wait();
            Teams.PurgeForTest().Wait();
            Seasons.PurgeForTest().Wait();
            Leagues.PurgeForTest().Wait();
        }


        /// <summary>
        /// Asserts the values of the properties for the two objects
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertProperties(object expected, object actual, params string[] toSkip)
        {
            Type ot = actual.GetType();
            PropertyInfo[] expectedProps = expected.GetType().GetProperties();
            foreach (PropertyInfo prop in expectedProps)
            {
                if (toSkip.Contains(prop.Name))
                {
                    continue;
                }

                PropertyInfo other = ot.GetProperty(prop.Name);
                if (other == null)
                {
                    Assert.Fail("Missing property: " + prop.Name);
                }

                object v1 = null;
                object v2 = null;

                try
                {
                    v1 = prop.GetValue(expected, null);
                    v2 = other.GetValue(actual, null);
                }
                catch
                {
                    Assert.Fail(prop.Name);
                }

                Assert.AreEqual(v1, v2, prop.Name);
            }
        }

        public static IEnumerable<League> InsertLeagues()
        {
            IRepositoryCollection collection = Collection();
            ILeagueRepository leagueRepository = collection.Leagues();
            IEnumerable<League> existing = leagueRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            List<League> leagues = new List<League>();

            leagues.Add(new League
            {
                Name = "National Hockey League",
                Abbreviation = "NHL",
            });

            leagues.Add(new League
            {
                Name = "Major League Baseball",
                Abbreviation = "MLB",
            });

            leagues.Add(new League
            {
                Name = "National Basketball Association",
                Abbreviation = "NBA",
            });

            leagues.Add(new League
            {
                Name = "National Football League",
                Abbreviation = "NFL",
            });

            foreach (League league in leagues)
            {
                leagueRepository
                    .Insert(league)
                    .Wait();
            }

            return leagues.OrderBy(leg => leg.Name);
        }

        public static IEnumerable<Athlete> InsertAthletes()
        {
            IRepositoryCollection collection = Collection();
            IAthleteRepository athleteRepository = collection.Athletes();
            IEnumerable<Athlete> existing = athleteRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            List<Athlete> athletes = new List<Athlete>();

            athletes.Add(new Athlete
            {
                Name = "Wayne Gretzky",
                BirthDate = new DateTime(1965, 02, 04),
                BirthCity = "Brandon",
                BirthCountry = "Ontario",
                Position = "C",
                Height = "6'0",
                Weight = "185"
            });

            athletes.Add(new Athlete
            {
                Name = "Bobby Orr",
                BirthDate = new DateTime(1941, 03, 06),
                BirthCity = "Newmarket",
                BirthCountry = "Ontario",
                Position = "D",
                Height = "6'0",
                Weight = "190"
            });

            athletes.Add(new Athlete
            {
                Name = "Mario Leimiuex",
                BirthDate = new DateTime(1967, 06, 02),
                BirthCity = "Montreal",
                BirthCountry = "Quebec",
                Position = "C",
                Height = "6'4",
                Weight = "220"
            });

            athletes.Add(new Athlete
            {
                Name = "Mark Messier",
                BirthDate = new DateTime(1966, 08, 04),
                BirthCity = "Edmonton",
                BirthCountry = "Alberta",
                Position = "C",
                Height = "6'0",
                Weight = "210"
            });

            foreach (Athlete athlete in athletes)
            {
                athleteRepository
                    .Insert(athlete)
                    .Wait();
            }

            return athletes.OrderBy(ath => ath.Name);
        }

        public static IEnumerable<Award> InsertAwards()
        {
            IRepositoryCollection collection = Collection();
            IAwardRepository awardRepository = collection.Awards();
            IEnumerable<Award> existing = awardRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<League> leagues = InsertLeagues();

            List<Award> awards = new List<Award>();

            awards.Add(new Award
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "Maurice Award",
                Abbreviation = "Maurice",
                StartDate = new DateTime(1990, 1, 1),
                EndDate = new DateTime(2015, 4, 5)
            });

            awards.Add(new Award
            {
                LeagueId = leagues.ElementAt(1).Id,
                Name = "Good Stuff Award",
                Abbreviation = "Stuff",
                StartDate = new DateTime(1930, 1, 1)
            });

            awards.Add(new Award
            {
                LeagueId = leagues.ElementAt(2).Id,
                Name = "Eat lots award",
                Abbreviation = "Eat",
                StartDate = new DateTime(1974, 1, 1)
            });

            awards.Add(new Award
            {
                LeagueId = leagues.ElementAt(3).Id,
                Name = "Memorial Tribute",
                Abbreviation = "memorial",
                StartDate = new DateTime(1945, 1, 1)
            });

            foreach (Award award in awards)
            {
                awardRepository
                    .Insert(award)
                    .Wait();
            }

            return awards.OrderBy(leg => leg.Name);
        }

        public static IEnumerable<Season> InsertSeasons()
        {
            IRepositoryCollection collection = Collection();
            ISeasonRepository seasonRepository = collection.Seasons();
            IEnumerable<Season> existing = seasonRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<League> leagues = InsertLeagues();

            List<Season> seasons = new List<Season>();

            seasons.Add(new Season
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "2004-2005 NHL Regular Season",
                StartDate = new DateTime(2004, 09, 01),
                EndDate = new DateTime(2005, 04, 10),
                IsPlayoffs = 0
            });

            seasons.Add(new Season
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "2004-2005 NHL Playoffs",
                StartDate = new DateTime(2004, 04, 13),
                EndDate = new DateTime(2005, 06, 02),
                IsPlayoffs = 1
            });

            seasons.Add(new Season
            {
                LeagueId = leagues.ElementAt(1).Id,
                Name = "2014-2015 NFL Regular Season",
                StartDate = new DateTime(2014, 09, 10),
                EndDate = new DateTime(2015, 04, 13),
                IsPlayoffs = 0
            });

            seasons.Add(new Season
            {
                LeagueId = leagues.ElementAt(2).Id,
                Name = "2009-2010 NBA Regular Season",
                StartDate = new DateTime(2009, 09, 02),
                EndDate = new DateTime(2010, 04, 12),
                IsPlayoffs = 0
            });

            foreach (Season season in seasons)
            {
                seasonRepository
                    .Insert(season)
                    .Wait();
            }

            return seasons.OrderBy(leg => leg.Name);
        }

        public static IEnumerable<Team> InsertTeams()
        {
            IRepositoryCollection collection = Collection();
            ITeamRepository teamRepository = collection.Teams();
            IEnumerable<Team> existing = teamRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<League> leagues = InsertLeagues();

            List<Team> teams = new List<Team>();

            teams.Add(new Team
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "Calgary Flames"
            });

            teams.Add(new Team
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "Edmonton Oilers"
            });

            teams.Add(new Team
            {
                LeagueId = leagues.ElementAt(1).Id,
                Name = "Pittsburgh Steelers"
            });

            teams.Add(new Team
            {
                LeagueId = leagues.ElementAt(2).Id,
                Name = "Toronto Blue Jays"
            });

            foreach (Team team in teams)
            {
                teamRepository
                    .Insert(team)
                    .Wait();
            }

            return teams.OrderBy(leg => leg.Name);
        }

        public static IEnumerable<TeamIdentity> InsertTeamIdentities()
        {
            IRepositoryCollection collection = Collection();
            ITeamIdentityRepository teamIdentityRepository = collection.TeamIdentities();
            IEnumerable<TeamIdentity> existing = teamIdentityRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Team> teams = InsertTeams();

            List<TeamIdentity> teamIdentities = new List<TeamIdentity>();

            teamIdentities.Add(new TeamIdentity
            {
                TeamId = teams.ElementAt(0).Id,
                Name = "Poops",
                Abbreviation = "CGY",
                City = "Calgary",
                StartDate = new DateTime(1970, 1, 1),
                EndDate = new DateTime(2000, 5, 6)
            });

            teamIdentities.Add(new TeamIdentity
            {
                TeamId = teams.ElementAt(0).Id,
                Name = "Wankers",
                Abbreviation = "Ari",
                City = "Arizona",
                StartDate = new DateTime(2000, 5, 6)
            });

            teamIdentities.Add(new TeamIdentity
            {
                TeamId = teams.ElementAt(1).Id,
                Name = "Farts",
                Abbreviation = "BMG",
                City = "Birmingham",
                StartDate = new DateTime(1980, 3, 1)
            });

            teamIdentities.Add(new TeamIdentity
            {
                TeamId = teams.ElementAt(2).Id,
                Name = "Twisters",
                Abbreviation = "PLO",
                City = "Kansas",
                StartDate = new DateTime(1934, 9, 10)
            });
            foreach (TeamIdentity teamIdentity in teamIdentities)
            {
                teamIdentityRepository
                    .Insert(teamIdentity)
                    .Wait();
            }

            return teamIdentities.OrderBy(leg => leg.Name);
        }

        public static IEnumerable<Conference> InsertConferences()
        {
            IRepositoryCollection collection = Collection();
            IConferenceRepository conferenceRepository = collection.Conferences();
            IEnumerable<Conference> existing = conferenceRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<League> leagues = InsertLeagues();
            List<Conference> conferences = new List<Conference>();

            conferences.Add(new Conference
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "West",
                StartYear = 2012,
                EndYear = null
            });

            conferences.Add(new Conference
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "East",
                StartYear = 2009,
                EndYear = 2012
            });

            conferences.Add(new Conference
            {
                LeagueId = leagues.ElementAt(0).Id,
                Name = "North",
                StartYear = 2012,
                EndYear = null
            });

            conferences.Add(new Conference
            {
                LeagueId = leagues.ElementAt(1).Id,
                Name = "South",
                StartYear = 2012,
                EndYear = null
            });

            foreach (Conference conference in conferences)
            {
                conferenceRepository
                    .Insert(conference)
                    .Wait();
            }

            return conferences.OrderBy(leg => leg.StartYear).ThenBy(leg => leg.Name);
        }

        public static IEnumerable<Division> InsertDivisions()
        {
            IRepositoryCollection collection = Collection();
            IDivisionRepository divisionRepository = collection.Divisions();
            IEnumerable<Division> existing = divisionRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Conference> conferences = InsertConferences();
            List<Division> divisions = new List<Division>();

            divisions.Add(new Division
            {
                ConferenceId = conferences.ElementAt(0).Id,
                Name = "West",
                StartYear = 2012,
                EndYear = null
            });

            divisions.Add(new Division
            {
                ConferenceId = conferences.ElementAt(0).Id,
                Name = "East",
                StartYear = 2009,
                EndYear = 2012
            });

            divisions.Add(new Division
            {
                ConferenceId = conferences.ElementAt(0).Id,
                Name = "North",
                StartYear = 2012,
                EndYear = null
            });

            divisions.Add(new Division
            {
                ConferenceId = conferences.ElementAt(1).Id,
                Name = "South",
                StartYear = 2012,
                EndYear = null
            });

            foreach (Division division in divisions)
            {
                divisionRepository
                    .Insert(division)
                    .Wait();
            }

            return divisions.OrderBy(leg => leg.StartYear).ThenBy(leg => leg.Name);
        }

        public static IEnumerable<Draft> InsertDrafts()
        {
            IRepositoryCollection collection = Collection();
            IDraftRepository draftRepository = collection.Drafts();
            IEnumerable<Draft> existing = draftRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Athlete> athletes = InsertAthletes();
            IEnumerable<TeamIdentity> teamIdentities = InsertTeamIdentities();

            List<Draft> drafts = new List<Draft>();

            drafts.Add(new Draft
            {
                AthleteId = athletes.ElementAt(0).Id,
                TeamIdentityId = teamIdentities.ElementAt(0).Id,
                Year = 2012,
                Round = 1,
                Position = 1
            });

            drafts.Add(new Draft
            {
                AthleteId = athletes.ElementAt(1).Id,
                TeamIdentityId = teamIdentities.ElementAt(1).Id,
                Year = 2012,
                Round = 1,
                Position = 2
            });


            drafts.Add(new Draft
            {
                AthleteId = athletes.ElementAt(2).Id,
                TeamIdentityId = teamIdentities.ElementAt(2).Id,
                Year = 2015,
                Round = 7,
                Position = 202
            });

            drafts.Add(new Draft
            {
                AthleteId = athletes.ElementAt(3).Id,
                TeamIdentityId = teamIdentities.ElementAt(3).Id,
                Year = 1937,
                Round = 5,
                Position = 32
            });

            foreach (Draft draft in drafts)
            {
                draftRepository
                    .Insert(draft)
                    .Wait();
            }

            return drafts.OrderBy(leg => leg.AthleteId);
        }

        public static IEnumerable<JerseyNumber> InsertJerseyNumbers()
        {
            IRepositoryCollection collection = Collection();
            IJerseyNumberRepository jerseyNumberRepository = collection.JerseyNumbers();
            IEnumerable<JerseyNumber> existing = jerseyNumberRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Athlete> athletes = InsertAthletes();
            IEnumerable<TeamIdentity> teamIdentities = InsertTeamIdentities();

            List<JerseyNumber> jerseyNumbers = new List<JerseyNumber>();

            jerseyNumbers.Add(new JerseyNumber
            {
                AthleteId = athletes.ElementAt(0).Id,
                TeamIdentityId = teamIdentities.ElementAt(0).Id,
                Number = 99,
                StartYear = 1979,
                EndYear = 1999
            });

            jerseyNumbers.Add(new JerseyNumber
            {
                AthleteId = athletes.ElementAt(1).Id,
                TeamIdentityId = teamIdentities.ElementAt(1).Id,
                Number = 19,
                StartYear = 1926,
                EndYear = 1936
            });

            jerseyNumbers.Add(new JerseyNumber
            {
                AthleteId = athletes.ElementAt(2).Id,
                TeamIdentityId = teamIdentities.ElementAt(2).Id,
                Number = 34,
                StartYear = 2014,
                EndYear = 2016
            });

            jerseyNumbers.Add(new JerseyNumber
            {
                AthleteId = athletes.ElementAt(3).Id,
                TeamIdentityId = teamIdentities.ElementAt(3).Id,
                Number = 66,
                StartYear = 1984,
                EndYear = 2006
            });

            foreach (JerseyNumber jerseyNumber in jerseyNumbers)
            {
                jerseyNumberRepository
                    .Insert(jerseyNumber)
                    .Wait();
            }

            return jerseyNumbers.OrderBy(leg => leg.AthleteId);
        }

        public static IEnumerable<AthleteAward> InsertAthleteAwards()
        {
            IRepositoryCollection collection = Collection();
            IAthleteAwardRepository athleteAwardRepository = collection.AthleteAwards();
            IEnumerable<AthleteAward> existing = athleteAwardRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Athlete> athletes = InsertAthletes();
            IEnumerable<Award> awards = InsertAwards();
            IEnumerable<Season> seasons = InsertSeasons();

            List<AthleteAward> athleteAwards = new List<AthleteAward>();

            athleteAwards.Add(new AthleteAward
            {
                AthleteId = athletes.ElementAt(0).Id,
                AwardId = awards.ElementAt(0).Id,
                SeasonId = seasons.ElementAt(0).Id
            });

            athleteAwards.Add(new AthleteAward
            {
                AthleteId = athletes.ElementAt(0).Id,
                AwardId = awards.ElementAt(1).Id,
                SeasonId = seasons.ElementAt(1).Id
            });

            athleteAwards.Add(new AthleteAward
            {
                AthleteId = athletes.ElementAt(1).Id,
                AwardId = awards.ElementAt(2).Id,
                SeasonId = seasons.ElementAt(3).Id
            });

            athleteAwards.Add(new AthleteAward
            {
                AthleteId = athletes.ElementAt(2).Id,
                AwardId = awards.ElementAt(0).Id,
                SeasonId = seasons.ElementAt(2).Id
            });

            foreach (AthleteAward athleteAward in athleteAwards)
            {
                athleteAwardRepository
                    .Insert(athleteAward)
                    .Wait();
            }

            return athleteAwards.OrderBy(leg => leg.AwardId).ThenBy(leg => leg.AthleteId);
        }

        public static IEnumerable<TeamIdentityDivision> InsertTeamIdentityDivisions()
        {
            IRepositoryCollection collection = Collection();
            ITeamIdentityDivisionRepository teamIdentityDivisionRepository = collection.TeamIdentityDivisions();
            IEnumerable<TeamIdentityDivision> existing = teamIdentityDivisionRepository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Division> divisions = InsertDivisions();
            IEnumerable<TeamIdentity> teamIdentities = InsertTeamIdentities();
            List<TeamIdentityDivision> teamIdentityDivisions = new List<TeamIdentityDivision>();

            teamIdentityDivisions.Add(new TeamIdentityDivision
            {
                TeamIdentityId = teamIdentities.ElementAt(0).Id,
                DivisionId = divisions.ElementAt(0).Id,
                StartYear = 2012,
                EndYear = null
            });

            teamIdentityDivisions.Add(new TeamIdentityDivision
            {
                TeamIdentityId = teamIdentities.ElementAt(1).Id,
                DivisionId = divisions.ElementAt(1).Id,
                StartYear = 1977,
                EndYear = 1993
            });

            teamIdentityDivisions.Add(new TeamIdentityDivision
            {
                TeamIdentityId = teamIdentities.ElementAt(2).Id,
                DivisionId = divisions.ElementAt(2).Id,
                StartYear = 2011,
                EndYear = 2012
            });

            teamIdentityDivisions.Add(new TeamIdentityDivision
            {
                TeamIdentityId = teamIdentities.ElementAt(3).Id,
                DivisionId = divisions.ElementAt(3).Id,
                StartYear = 2000,
                EndYear = 2009
            });

            foreach (TeamIdentityDivision division in teamIdentityDivisions)
            {
                teamIdentityDivisionRepository
                    .Insert(division)
                    .Wait();
            }

            return teamIdentityDivisions.OrderBy(leg => leg.StartYear);
        }

        public static IEnumerable<GoalieStatLine> InsertGoalieStatLines()
        {
            IRepositoryCollection collection = Collection();
            IGoalieStatLineRepository repository = collection.GoalieStatLines();
            IEnumerable<GoalieStatLine> existing = repository.List().Result;
            if (existing.Count() > 0)
            {
                return existing;
            }

            IEnumerable<Athlete> athletes = InsertAthletes();
            IEnumerable<Season> seasons = InsertSeasons();
            IEnumerable<TeamIdentity> teamIdentities = InsertTeamIdentities();
            List<GoalieStatLine> goalieStatLines = new List<GoalieStatLine>();

            goalieStatLines.Add(new GoalieStatLine
            {
                AthleteId = athletes.ElementAt(0).Id,
                TeamIdentityId = teamIdentities.ElementAt(0).Id,
                SeasonId = seasons.ElementAt(0).Id,
                GamesPlayed = 67,
                GamesStarted = 64,
                Wins = 20,
                Losses = 40,
                TiesPlusOvertimeShootoutLosses = 4,
                GoalsAgainst = 180,
                ShotsAgainst = 1998,
                Saves = 1100,
                SavePercentage = 0.912M,
                GoalsAgainstAverage = 2.87M,
                Shutouts = 5,
                Minutes = 3600,
                QualityStarts = 20,
                QualityStartPercentage = 0.34M,
                ReallyBadStarts = 20,
                GoalsAgainstPercentage = 0.51M,
                GoalsSavedAboveAverage = 0.23M,
                GoaliePointShares = 0.75M,
                Goals = 2,
                Assists = 4,
                PenaltyMinutes = 123,
                StanleyCup = 0,
                IsPlayoffs = 0
            });

            goalieStatLines.Add(new GoalieStatLine
            {
                AthleteId = athletes.ElementAt(0).Id,
                TeamIdentityId = teamIdentities.ElementAt(1).Id,
                SeasonId = seasons.ElementAt(2).Id,
                GamesPlayed = 43,
                GamesStarted = 23,
                Wins = 10,
                Losses = 20,
                TiesPlusOvertimeShootoutLosses = 8,
                GoalsAgainst = 100,
                ShotsAgainst = 800,
                Saves = 700,
                SavePercentage = 0.822M,
                GoalsAgainstAverage = 3.25M,
                Shutouts = 0,
                Minutes = 2200,
                QualityStarts = 5,
                QualityStartPercentage = 0.10M,
                ReallyBadStarts = 34,
                GoalsAgainstPercentage = 0.19M,
                GoalsSavedAboveAverage = 0.09M,
                GoaliePointShares = 0.05M,
                Goals = 10,
                Assists = 20,
                PenaltyMinutes = 562,
                StanleyCup = 1,
                IsPlayoffs = 1
            });

            goalieStatLines.Add(new GoalieStatLine
            {
                AthleteId = athletes.ElementAt(3).Id,
                TeamIdentityId = teamIdentities.ElementAt(2).Id,
                SeasonId = seasons.ElementAt(1).Id,
                GamesPlayed = 43,
                GamesStarted = 23,
                Wins = 10,
                Losses = 20,
                TiesPlusOvertimeShootoutLosses = 8,
                GoalsAgainst = 100,
                ShotsAgainst = 800,
                Saves = 700,
                SavePercentage = 0.822M,
                GoalsAgainstAverage = 3.25M,
                Shutouts = 0,
                Minutes = 2200,
                QualityStarts = 5,
                QualityStartPercentage = 0.10M,
                ReallyBadStarts = 34,
                GoalsAgainstPercentage = 0.19M,
                GoalsSavedAboveAverage = 0.09M,
                GoaliePointShares = 0.05M,
                Goals = 10,
                Assists = 20,
                PenaltyMinutes = 562,
                StanleyCup = 1,
                IsPlayoffs = 1
            });

            goalieStatLines.Add(new GoalieStatLine
            {
                AthleteId = athletes.ElementAt(0).Id,
                TeamIdentityId = teamIdentities.ElementAt(3).Id,
                SeasonId = seasons.ElementAt(1).Id,
                GamesPlayed = 43,
                GamesStarted = 23,
                Wins = 10,
                Losses = 20,
                TiesPlusOvertimeShootoutLosses = 8,
                GoalsAgainst = 100,
                ShotsAgainst = 800,
                Saves = 700,
                SavePercentage = 0.822M,
                GoalsAgainstAverage = 3.25M,
                Shutouts = 0,
                Minutes = 2200,
                QualityStarts = 5,
                QualityStartPercentage = 0.10M,
                ReallyBadStarts = 34,
                GoalsAgainstPercentage = 0.19M,
                GoalsSavedAboveAverage = 0.09M,
                GoaliePointShares = 0.05M,
                Goals = 10,
                Assists = 20,
                PenaltyMinutes = 562,
                StanleyCup = 1,
                IsPlayoffs = 1
            });

            foreach (GoalieStatLine goalieStatLine in goalieStatLines)
            {
                repository
                    .Insert(goalieStatLine)
                    .Wait();
            }

            return goalieStatLines.OrderBy(leg => leg.AthleteId);
        }
    }
}
