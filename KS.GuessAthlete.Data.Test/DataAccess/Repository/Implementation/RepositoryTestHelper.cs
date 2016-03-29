﻿using KS.GuessAthlete.Component.Caching.Implementation;
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

            leagueRepository.Insert(new League
            {
                Name = "National Hockey League",
                Abbreviation = "NHL",
            }).Wait();

            leagueRepository.Insert(new League
            {
                Name = "Major League Baseball",
                Abbreviation = "MLB",
            }).Wait();

            leagueRepository.Insert(new League
            {
                Name = "National Basketball Association",
                Abbreviation = "NBA",
            }).Wait();

            leagueRepository.Insert(new League
            {
                Name = "National Football League",
                Abbreviation = "NFL",
            }).Wait();

            return leagueRepository.List().Result;
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

            // Act
            gameRepository.Insert(game1).Wait();
            gameRepository.Insert(game2).Wait();
            gameRepository.Insert(game3).Wait();
            gameRepository.Insert(game4).Wait();

            return gameRepository.List().Result;
        }
        */
    }
}