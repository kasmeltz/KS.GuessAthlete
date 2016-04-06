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
    /// Unit Tests for the DapperSkaterStatLineRepository
    /// </summary>
    [TestClass]
    public class DapperSkaterStatLineRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void SkaterStatLineRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ISkaterStatLineRepository skaterStatLineRepository = collection.SkaterStatLines();
            IEnumerable<SkaterStatLine> insertedSkaterStatLines = RepositoryTestHelper.InsertSkaterStatLines();
            IEnumerable<SkaterStatLine> listedSkaterStatLines = skaterStatLineRepository.List().Result;

            IEnumerable<Athlete> athletes = RepositoryTestHelper.InsertAthletes();
            IEnumerable<TeamIdentity> teamIdentities = RepositoryTestHelper.InsertTeamIdentities();
            IEnumerable<Season> seasons = RepositoryTestHelper.InsertSeasons();

            for (int i = 0; i < listedSkaterStatLines.Count(); i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedSkaterStatLines.ElementAt(i),
                        listedSkaterStatLines.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                skaterStatLineRepository.Insert(insertedSkaterStatLines.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            /*
            listedSkaterStatLines.ElementAt(2).AthleteId = athletes.ElementAt(3).Id;
            listedSkaterStatLines.ElementAt(2).TeamIdentityId = teamIdentities.ElementAt(3).Id;
            listedSkaterStatLines.ElementAt(2).SeasonId = seasons.ElementAt(3).Id;
            listedSkaterStatLines.ElementAt(2).GamesPlayed = 192;
            listedSkaterStatLines.ElementAt(2).GamesStarted = 134;
            listedSkaterStatLines.ElementAt(2).Wins = 126;
            listedSkaterStatLines.ElementAt(2).Losses = 76;
            listedSkaterStatLines.ElementAt(2).TiesPlusOvertimeShootoutLosses = 297;
            listedSkaterStatLines.ElementAt(2).GoalsAgainst = 5;
            listedSkaterStatLines.ElementAt(2).ShotsAgainst = 5678;
            listedSkaterStatLines.ElementAt(2).Saves = 5673;
            listedSkaterStatLines.ElementAt(2).SavePercentage = 0.998M;
            listedSkaterStatLines.ElementAt(2).GoalsAgainstAverage = 0.002M;
            listedSkaterStatLines.ElementAt(2).Shutouts = 3000; ;
            listedSkaterStatLines.ElementAt(2).Minutes = 65789;
            listedSkaterStatLines.ElementAt(2).QualityStarts = 2;
            listedSkaterStatLines.ElementAt(2).QualityStartPercentage = 0.00001M;
            listedSkaterStatLines.ElementAt(2).ReallyBadStarts = 6230;
            listedSkaterStatLines.ElementAt(2).GoalsAgainstPercentage = 14.36M;
            listedSkaterStatLines.ElementAt(2).GoalsSavedAboveAverage = 20.3M;
            listedSkaterStatLines.ElementAt(2).GoaliePointShares = 1000;
            listedSkaterStatLines.ElementAt(2).Goals = 500;
            listedSkaterStatLines.ElementAt(2).Assists = 2;
            listedSkaterStatLines.ElementAt(2).PenaltyMinutes = 1;
            listedSkaterStatLines.ElementAt(2).StanleyCup = 0;
            listedSkaterStatLines.ElementAt(2).IsPlayoffs = 0;

            skaterStatLineRepository.Update(listedSkaterStatLines.ElementAt(2)).Wait();
            SkaterStatLine updatedSkaterStatLine = skaterStatLineRepository.Get(listedSkaterStatLines.ElementAt(2).Id).Result;

            RepositoryTestHelper
                    .AssertProperties(listedSkaterStatLines.ElementAt(2),
                        updatedSkaterStatLine);

            exceptionThrown = false;
            try
            {
                listedSkaterStatLines.ElementAt(3).AthleteId = athletes.ElementAt(3).Id;
                listedSkaterStatLines.ElementAt(3).TeamIdentityId = teamIdentities.ElementAt(3).Id;
                listedSkaterStatLines.ElementAt(3).SeasonId = seasons.ElementAt(3).Id;
                skaterStatLineRepository.Update(listedSkaterStatLines.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedSkaterStatLine = skaterStatLineRepository.Get(listedSkaterStatLines.ElementAt(3).Id).Result;
            Assert.AreEqual(athletes.ElementAt(0).Id, updatedSkaterStatLine.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(3).Id, updatedSkaterStatLine.TeamIdentityId);
            Assert.AreEqual(seasons.ElementAt(1).Id, updatedSkaterStatLine.SeasonId);
            */

            skaterStatLineRepository.Delete(listedSkaterStatLines.ElementAt(0).Id).Wait();
            listedSkaterStatLines = skaterStatLineRepository.List().Result;
            Assert.AreEqual(3, listedSkaterStatLines.Count());
        }
    }
}
