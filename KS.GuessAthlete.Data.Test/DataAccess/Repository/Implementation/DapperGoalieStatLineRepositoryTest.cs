using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperGoalieStatLineRepository
    /// </summary>
    [TestClass]
    public class DapperGoalieStatLineRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void GoalieStatLineRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IGoalieStatLineRepository goalieStatLineRepository = collection.GoalieStatLines();
            IEnumerable<GoalieStatLine> insertedGoalieStatLines = RepositoryTestHelper.InsertGoalieStatLines();
            IEnumerable<GoalieStatLine> listedGoalieStatLines = goalieStatLineRepository.List().Result;

            IEnumerable<Athlete> athletes = RepositoryTestHelper.InsertAthletes();
            IEnumerable<TeamIdentity> teamIdentities = RepositoryTestHelper.InsertTeamIdentities();
            IEnumerable<Season> seasons = RepositoryTestHelper.InsertSeasons();

            for (int i = 0; i < listedGoalieStatLines.Count(); i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedGoalieStatLines.ElementAt(i),
                        listedGoalieStatLines.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                goalieStatLineRepository.Insert(insertedGoalieStatLines.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            listedGoalieStatLines.ElementAt(2).AthleteId = athletes.ElementAt(3).Id;
            listedGoalieStatLines.ElementAt(2).TeamIdentityId = teamIdentities.ElementAt(3).Id;
            listedGoalieStatLines.ElementAt(2).SeasonId = seasons.ElementAt(3).Id;
            listedGoalieStatLines.ElementAt(2).GamesPlayed = 192;
            listedGoalieStatLines.ElementAt(2).GamesStarted = 134;
            listedGoalieStatLines.ElementAt(2).Wins = 126;
            listedGoalieStatLines.ElementAt(2).Losses = 76;
            listedGoalieStatLines.ElementAt(2).TiesPlusOvertimeShootoutLosses = 297;
            listedGoalieStatLines.ElementAt(2).GoalsAgainst = 5;
            listedGoalieStatLines.ElementAt(2).ShotsAgainst = 5678;
            listedGoalieStatLines.ElementAt(2).Saves = 5673;
            listedGoalieStatLines.ElementAt(2).SavePercentage = 0.998M;
            listedGoalieStatLines.ElementAt(2).GoalsAgainstAverage = 0.002M;
            listedGoalieStatLines.ElementAt(2).Shutouts = 3000; ;
            listedGoalieStatLines.ElementAt(2).Minutes = 65789;
            listedGoalieStatLines.ElementAt(2).QualityStarts = 2;
            listedGoalieStatLines.ElementAt(2).QualityStartPercentage = 0.00001M;
            listedGoalieStatLines.ElementAt(2).ReallyBadStarts = 6230;
            listedGoalieStatLines.ElementAt(2).GoalsAgainstPercentage = 14.36M;
            listedGoalieStatLines.ElementAt(2).GoalsSavedAboveAverage = 20.3M;
            listedGoalieStatLines.ElementAt(2).GoaliePointShares = 1000;
            listedGoalieStatLines.ElementAt(2).Goals = 500;
            listedGoalieStatLines.ElementAt(2).Assists = 2;
            listedGoalieStatLines.ElementAt(2).PenaltyMinutes = 1;
            listedGoalieStatLines.ElementAt(2).StanleyCup = 0;
            listedGoalieStatLines.ElementAt(2).IsPlayoffs = 0;

            goalieStatLineRepository.Update(listedGoalieStatLines.ElementAt(2)).Wait();
            GoalieStatLine updatedGoalieStatLine = goalieStatLineRepository.Get(listedGoalieStatLines.ElementAt(2).Id).Result;

            RepositoryTestHelper
                    .AssertProperties(listedGoalieStatLines.ElementAt(2),
                        updatedGoalieStatLine);

            exceptionThrown = false;
            try
            {
                listedGoalieStatLines.ElementAt(3).AthleteId = athletes.ElementAt(3).Id;
                listedGoalieStatLines.ElementAt(3).TeamIdentityId = teamIdentities.ElementAt(3).Id;
                listedGoalieStatLines.ElementAt(3).SeasonId = seasons.ElementAt(3).Id;
                listedGoalieStatLines.ElementAt(3).IsPlayoffs = 0;
                goalieStatLineRepository.Update(listedGoalieStatLines.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedGoalieStatLine = goalieStatLineRepository.Get(listedGoalieStatLines.ElementAt(3).Id).Result;
            Assert.AreEqual(athletes.ElementAt(0).Id, updatedGoalieStatLine.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(3).Id, updatedGoalieStatLine.TeamIdentityId);
            Assert.AreEqual(seasons.ElementAt(1).Id, updatedGoalieStatLine.SeasonId);

            goalieStatLineRepository.Delete(listedGoalieStatLines.ElementAt(0).Id).Wait();
            listedGoalieStatLines = goalieStatLineRepository.List().Result;
            Assert.AreEqual(3, listedGoalieStatLines.Count());

            listedGoalieStatLines = goalieStatLineRepository.ForAthlete(athletes.ElementAt(0).Id).Result;
            Assert.AreEqual(2, listedGoalieStatLines.Count());
        }
    }
}
