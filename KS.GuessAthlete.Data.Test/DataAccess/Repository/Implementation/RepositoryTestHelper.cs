using KS.GuessAthlete.Component.Caching.Implementation;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Repository.Implementation;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
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
            IAwardRepository Awards  = collection.Awards();
            IDraftRepository Drafts = collection.Drafts();
            IGoalieStatLineRepository GoalieStatLines = collection.GoalieStatLines();
            IJerseyNumberRepository JerseyNumbers = collection.JerseyNumbers();
            ILeagueRepository Leagues = collection.Leagues();
            ISeasonRepository Seasons = collection.Seasons();
            ISkaterStatLineRepository SkaterStatLines = collection.SkaterStatLines();
            ITeamRepository Teams = collection.Teams();
            ITeamIdentityRepository TeamIdentities = collection.TeamIdentities();

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

            foreach(League league in leagues)
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
                StartDate = new DateTime(1990,1,1),
                EndDate = new DateTime(2015,4,5)
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
                TeamIdentityId  = teamIdentities.ElementAt(0).Id,
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


        /*
        public static IEnumerable<Game> InsertGames()
        {
            IRepositoryCollection collection = Collection();
            ITeamRepository teamRepository = collection.Teams();
            IEnumerable<Team> teams = teamRepository.List().Result;
            if (teams.Count() == 0)
            {
                teams = InsertTeams();
            }

            IGameRepository gameRepository = collection.Games();
            Game game1 = null;
            Game game2 = null;
            Game game3 = null;
            Game game4 = null;

            game1 = new Game
            {
                StartDate = new DateTime(2015, 01, 01),
                HomeTeamId = teams.ElementAt(0).Id,
                AwayTeamId = teams.ElementAt(1).Id,
            };

            game2 = new Game
            {
                StartDate = new DateTime(2015, 02, 14),
                HomeTeamId = teams.ElementAt(1).Id,
                AwayTeamId = teams.ElementAt(2).Id,
            };

            game3 = new Game
            {
                StartDate = new DateTime(2016, 02, 14),
                HomeTeamId = teams.ElementAt(1).Id,
                AwayTeamId = teams.ElementAt(2).Id,
            };


            game4 = new Game
            {
                StartDate = new DateTime(2016, 08, 12),
                HomeTeamId = teams.ElementAt(2).Id,
                AwayTeamId = teams.ElementAt(0).Id,
            };

            gameRepository.Insert(game1).Wait();
            gameRepository.Insert(game2).Wait();
            gameRepository.Insert(game3).Wait();
            gameRepository.Insert(game4).Wait();

            return gameRepository.List().Result;
        }
        */
        }
    }
