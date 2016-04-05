using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperAthleteAwardRepository
    /// </summary>
    [TestClass]
    public class DapperAthleteAwardRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void AthleteAwardRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IAthleteAwardRepository athleteAwardRepository = collection.AthleteAwards();
            IEnumerable<AthleteAward> insertedAthleteAwards = RepositoryTestHelper.InsertAthleteAwards();
            IEnumerable<AthleteAward> listedAthleteAwards = athleteAwardRepository.List().Result;

            IEnumerable<Athlete> athletes = RepositoryTestHelper.InsertAthletes();
            IEnumerable<Award> awards = RepositoryTestHelper.InsertAwards();
            IEnumerable<Season> seasons = RepositoryTestHelper.InsertSeasons();

            for (int i = 0;i < listedAthleteAwards.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedAthleteAwards.ElementAt(i), 
                        listedAthleteAwards.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                athleteAwardRepository.Insert(insertedAthleteAwards.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedAthleteAwards.ElementAt(2).AthleteId = athletes.ElementAt(1).Id;
            listedAthleteAwards.ElementAt(2).AwardId = awards.ElementAt(3).Id;
            listedAthleteAwards.ElementAt(2).SeasonId = seasons.ElementAt(3).Id;
            athleteAwardRepository.Update(listedAthleteAwards.ElementAt(2)).Wait();

            AthleteAward updatedAthleteAward = athleteAwardRepository.Get(listedAthleteAwards.ElementAt(2).Id).Result;
            Assert.AreEqual(athletes.ElementAt(1).Id, updatedAthleteAward.AthleteId);
            Assert.AreEqual(awards.ElementAt(3).Id, updatedAthleteAward.AwardId);
            Assert.AreEqual(seasons.ElementAt(3).Id, updatedAthleteAward.SeasonId);

            exceptionThrown = false;
            try
            {
                listedAthleteAwards.ElementAt(3).AthleteId = athletes.ElementAt(1).Id;
                listedAthleteAwards.ElementAt(3).AwardId = awards.ElementAt(3).Id;
                listedAthleteAwards.ElementAt(3).SeasonId = seasons.ElementAt(3).Id;
                athleteAwardRepository.Update(listedAthleteAwards.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedAthleteAward = athleteAwardRepository.Get(listedAthleteAwards.ElementAt(3).Id).Result;
            Assert.AreEqual(athletes.ElementAt(2).Id, updatedAthleteAward.AthleteId);
            Assert.AreEqual(awards.ElementAt(0).Id, updatedAthleteAward.AwardId);
            Assert.AreEqual(seasons.ElementAt(2).Id, updatedAthleteAward.SeasonId);

            athleteAwardRepository.Delete(listedAthleteAwards.ElementAt(0).Id).Wait();
            listedAthleteAwards = athleteAwardRepository.List().Result;
            Assert.AreEqual(3, listedAthleteAwards.Count());
        }
    }
}
