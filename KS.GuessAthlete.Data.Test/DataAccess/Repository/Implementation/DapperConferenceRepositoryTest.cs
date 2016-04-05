using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperConferenceRepository
    /// </summary>
    [TestClass]
    public class DapperConferenceRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void ConferenceRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IConferenceRepository conferenceRepository = collection.Conferences();
            IEnumerable<Conference> insertedConferences = RepositoryTestHelper.InsertConferences();
            IEnumerable<Conference> listedConferences = conferenceRepository.List().Result;

            IEnumerable<League> leagues = RepositoryTestHelper.InsertLeagues();

            for(int i = 0;i < listedConferences.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedConferences.ElementAt(i), 
                        listedConferences.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                conferenceRepository.Insert(insertedConferences.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedConferences.ElementAt(2).LeagueId = leagues.ElementAt(3).Id;
            listedConferences.ElementAt(2).Name = "Soujteast";
            listedConferences.ElementAt(2).StartYear = 1987;
            listedConferences.ElementAt(2).EndYear = 1996;
            conferenceRepository.Update(listedConferences.ElementAt(2)).Wait();

            Conference updatedConference = conferenceRepository.Get(listedConferences.ElementAt(2).Id).Result;
            Assert.AreEqual(leagues.ElementAt(3).Id, updatedConference.LeagueId);
            Assert.AreEqual("Soujteast", updatedConference.Name);
            Assert.AreEqual(1987, updatedConference.StartYear);
            Assert.AreEqual(1996, updatedConference.EndYear);

            exceptionThrown = false;
            try
            {
                listedConferences.ElementAt(3).Name = "Soujteast";
                listedConferences.ElementAt(3).StartYear = 1987;
                conferenceRepository.Update(listedConferences.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedConference = conferenceRepository.Get(listedConferences.ElementAt(3).Id).Result;
            Assert.AreEqual(leagues.ElementAt(0).Id, updatedConference.LeagueId);
            Assert.AreEqual("West", updatedConference.Name);
            Assert.AreEqual(2012, updatedConference.StartYear);
            Assert.AreEqual(null, updatedConference.EndYear);

            conferenceRepository.Delete(listedConferences.ElementAt(0).Id).Wait();
            listedConferences = conferenceRepository.List().Result;
            Assert.AreEqual(3, listedConferences.Count());
        }
    }
}
