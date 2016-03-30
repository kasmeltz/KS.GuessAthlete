using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperAwardRepository
    /// </summary>
    [TestClass]
    public class DapperAwardRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void AwardRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IAwardRepository awardRepository = collection.Awards();
            IEnumerable<Award> insertedAwards = RepositoryTestHelper.InsertAwards();
            IEnumerable<Award> listedAwards = awardRepository.List().Result;

            for(int i = 0;i < listedAwards.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedAwards.ElementAt(i), 
                        listedAwards.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                awardRepository.Insert(insertedAwards.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            listedAwards.ElementAt(2).Name = "New Award Name";
            listedAwards.ElementAt(2).Abbreviation = "NLN";
            listedAwards.ElementAt(2).StartDate = new DateTime(2000, 1, 1);
            listedAwards.ElementAt(2).EndDate = new DateTime(2015, 1, 1);
            awardRepository.Update(listedAwards.ElementAt(2)).Wait();

            Award updatedAward = awardRepository.Get(listedAwards.ElementAt(2).Id).Result;

            Assert.AreEqual("New Award Name", updatedAward.Name);
            Assert.AreEqual("NLN", updatedAward.Abbreviation);
            Assert.AreEqual(new DateTime(2000, 1, 1), updatedAward.StartDate);
            Assert.AreEqual(new DateTime(2015, 1, 1), updatedAward.EndDate);

            exceptionThrown = false;
            try
            {
                listedAwards.ElementAt(3).Name = "New Award Name";
                listedAwards.ElementAt(3).LeagueId = listedAwards.ElementAt(2).LeagueId;
                awardRepository.Update(listedAwards.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedAward = awardRepository.Get(listedAwards.ElementAt(3).Id).Result;
            Assert.AreEqual("Memorial Tribute", updatedAward.Name);
            Assert.AreEqual("memorial", updatedAward.Abbreviation);
            Assert.AreEqual(new DateTime(1945, 1, 1), updatedAward.StartDate);
            Assert.AreEqual(null, updatedAward.EndDate);

            awardRepository.Delete(listedAwards.ElementAt(0).Id).Wait();
            listedAwards = awardRepository.List().Result;
            Assert.AreEqual(3, listedAwards.Count());
        }
    }
}
