using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperTeamRepository
    /// </summary>
    [TestClass]
    public class DapperTeamRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void TeamRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ITeamRepository teamRepository = collection.Teams();
            IEnumerable<Team> insertedTeams = RepositoryTestHelper.InsertTeams();
            IEnumerable<Team> listedTeams = teamRepository.List().Result;

            IEnumerable<League> leagues = RepositoryTestHelper.InsertLeagues();

            for(int i = 0;i < listedTeams.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedTeams.ElementAt(i), 
                        listedTeams.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                teamRepository.Insert(insertedTeams.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedTeams.ElementAt(2).LeagueId = leagues.ElementAt(2).Id;
            listedTeams.ElementAt(2).Name = "Üpdated team namE";
            teamRepository.Update(listedTeams.ElementAt(2)).Wait();

            Team updatedTeam = teamRepository.Get(listedTeams.ElementAt(2).Id).Result;
            Assert.AreEqual(leagues.ElementAt(2).Id, updatedTeam.LeagueId);
            Assert.AreEqual("Üpdated team namE", updatedTeam.Name);

            exceptionThrown = false;
            try
            {
                listedTeams.ElementAt(3).LeagueId = leagues.ElementAt(2).Id;
                listedTeams.ElementAt(3).Name = "Üpdated team namE";
                teamRepository.Update(listedTeams.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedTeam = teamRepository.Get(listedTeams.ElementAt(3).Id).Result;
            Assert.AreEqual(leagues.ElementAt(2).Id, updatedTeam.LeagueId);
            Assert.AreEqual("Toronto Blue Jays", updatedTeam.Name);

            teamRepository.Delete(listedTeams.ElementAt(0).Id).Wait();
            listedTeams = teamRepository.List().Result;
            Assert.AreEqual(3, listedTeams.Count());
        }
    }
}
