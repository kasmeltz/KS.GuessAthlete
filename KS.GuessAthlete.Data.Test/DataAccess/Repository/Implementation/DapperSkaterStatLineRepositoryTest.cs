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

            listedSkaterStatLines.ElementAt(2).AthleteId = athletes.ElementAt(3).Id;
            listedSkaterStatLines.ElementAt(2).TeamIdentityId = teamIdentities.ElementAt(3).Id;
            listedSkaterStatLines.ElementAt(2).SeasonId = seasons.ElementAt(3).Id;
            listedSkaterStatLines.ElementAt(2).GamesPlayed = 192;
            listedSkaterStatLines.ElementAt(2).Goals = 500;
            listedSkaterStatLines.ElementAt(2).Assists = 2;
            listedSkaterStatLines.ElementAt(2).PlusMinus = 146;
            listedSkaterStatLines.ElementAt(2).PenaltyMinutes = 1;
            listedSkaterStatLines.ElementAt(2).EvenStrengthGoals = 33;
            listedSkaterStatLines.ElementAt(2).PowerPlayGoals = 44;
            listedSkaterStatLines.ElementAt(2).ShortHandedGoals = 55;
            listedSkaterStatLines.ElementAt(2).GameWinningGoals = 66;
            listedSkaterStatLines.ElementAt(2).EvenStrengthAssists = 77;
            listedSkaterStatLines.ElementAt(2).PowerPlayAssists = 88;
            listedSkaterStatLines.ElementAt(2).ShortHandedAssists = 99;
            listedSkaterStatLines.ElementAt(2).Shots = 111;
            listedSkaterStatLines.ElementAt(2).ShotPercentage = 0.235M;
            listedSkaterStatLines.ElementAt(2).TimeOnIce = 200000;
            listedSkaterStatLines.ElementAt(2).AverageTimeOnIce = 2.4M;
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
            Assert.AreEqual(athletes.ElementAt(2).Id, updatedSkaterStatLine.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(1).Id, updatedSkaterStatLine.TeamIdentityId);
            Assert.AreEqual(seasons.ElementAt(0).Id, updatedSkaterStatLine.SeasonId);

            skaterStatLineRepository.Delete(listedSkaterStatLines.ElementAt(0).Id).Wait();
            listedSkaterStatLines = skaterStatLineRepository.List().Result;
            Assert.AreEqual(3, listedSkaterStatLines.Count());
        }
    }
}
