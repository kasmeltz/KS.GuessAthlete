using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperSeasonRepository
    /// </summary>
    [TestClass]
    public class DapperSeasonRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void SeasonRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ISeasonRepository seasonRepository = collection.Seasons();
            IEnumerable<Season> insertedSeasons = RepositoryTestHelper.InsertSeasons();
            IEnumerable<Season> listedSeasons = seasonRepository.List().Result;

            IEnumerable<League> leagues = RepositoryTestHelper.InsertLeagues();

            for (int i = 0; i < listedSeasons.Count(); i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedSeasons.ElementAt(i),
                        listedSeasons.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                seasonRepository.Insert(insertedSeasons.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            listedSeasons.ElementAt(2).LeagueId = leagues.ElementAt(2).Id;
            listedSeasons.ElementAt(2).Name = "Üpdated season namE";
            listedSeasons.ElementAt(2).StartDate = new DateTime(2000, 1, 1);
            listedSeasons.ElementAt(2).EndDate = new DateTime(2000, 12, 31);
            listedSeasons.ElementAt(2).IsPlayoffs = 1;
            seasonRepository.Update(listedSeasons.ElementAt(2)).Wait();

            Season updatedSeason = seasonRepository.Get(listedSeasons.ElementAt(2).Id).Result;
            Assert.AreEqual(leagues.ElementAt(2).Id, updatedSeason.LeagueId);
            Assert.AreEqual("Üpdated season namE", updatedSeason.Name);
            Assert.AreEqual(new DateTime(2000, 1, 1), updatedSeason.StartDate);
            Assert.AreEqual(new DateTime(2000, 12, 31), updatedSeason.EndDate);
            Assert.AreEqual(1, updatedSeason.IsPlayoffs);


            exceptionThrown = false;
            try
            {
                listedSeasons.ElementAt(3).LeagueId = leagues.ElementAt(2).Id;
                listedSeasons.ElementAt(3).Name = "Üpdated season namE";
                seasonRepository.Update(listedSeasons.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedSeason = seasonRepository.Get(listedSeasons.ElementAt(3).Id).Result;
            Assert.AreEqual(leagues.ElementAt(1).Id, updatedSeason.LeagueId);
            Assert.AreEqual("2014-2015 NFL Regular Season", updatedSeason.Name);
            Assert.AreEqual(new DateTime(2014, 9, 10), updatedSeason.StartDate);
            Assert.AreEqual(new DateTime(2015, 4, 13), updatedSeason.EndDate);
            Assert.AreEqual(0, updatedSeason.IsPlayoffs);

            seasonRepository.Delete(listedSeasons.ElementAt(0).Id).Wait();
            listedSeasons = seasonRepository.List().Result;
            Assert.AreEqual(3, listedSeasons.Count());
        }
    }
}
