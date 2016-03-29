using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperLeagueRepository
    /// </summary>
    [TestClass]
    public class DapperLeagueRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void LeagueRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ILeagueRepository leagueRepository = collection.Leagues();
            IEnumerable<League> insertedLeagues = RepositoryTestHelper.InsertLeagues();
            IEnumerable<League> listedLeagues = leagueRepository.List().Result;

            for(int i = 0;i < listedLeagues.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedLeagues.ElementAt(i), 
                        listedLeagues.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                // Act
                leagueRepository.Insert(insertedLeagues.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);

            listedLeagues.ElementAt(2).Name = "New League Name";
            listedLeagues.ElementAt(2).Abbreviation = "NLN";
            leagueRepository.Update(listedLeagues.ElementAt(2)).Wait();

            League updatedLeague = leagueRepository.Get(listedLeagues.ElementAt(2).Id).Result;

            Assert.AreEqual("New League Name", updatedLeague.Name);
            Assert.AreEqual("NLN", updatedLeague.Abbreviation);

            exceptionThrown = false;
            try
            {
                listedLeagues.ElementAt(3).Name = "New League Name";
                listedLeagues.ElementAt(3).Abbreviation = "NLN";
                leagueRepository.Update(listedLeagues.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            updatedLeague = leagueRepository.Get(listedLeagues.ElementAt(3).Id).Result;
            Assert.AreEqual("National Hockey League", updatedLeague.Name);
            Assert.AreEqual("NHL", updatedLeague.Abbreviation);

            leagueRepository.Delete(listedLeagues.ElementAt(0).Id);
            listedLeagues = leagueRepository.List().Result;
            Assert.AreEqual(3, listedLeagues.Count());
        }
    }
}
