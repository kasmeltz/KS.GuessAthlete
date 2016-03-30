using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperTeamIdentityRepository
    /// </summary>
    [TestClass]
    public class DapperTeamIdentityRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void TeamIdentityRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ITeamIdentityRepository teamIdentityRepository = collection.TeamIdentities();
            IEnumerable<TeamIdentity> insertedTeamIdentitys = RepositoryTestHelper.InsertTeamIdentities();
            IEnumerable<TeamIdentity> listedTeamIdentitys = teamIdentityRepository.List().Result;

            IEnumerable<Team> teams = RepositoryTestHelper.InsertTeams();

            for(int i = 0;i < listedTeamIdentitys.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedTeamIdentitys.ElementAt(i), 
                        listedTeamIdentitys.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                teamIdentityRepository.Insert(insertedTeamIdentitys.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedTeamIdentitys.ElementAt(2).TeamId = teams.ElementAt(1).Id;
            listedTeamIdentitys.ElementAt(2).Name = "NEW TEAM ID";
            listedTeamIdentitys.ElementAt(2).Abbreviation = "FU";
            listedTeamIdentitys.ElementAt(2).City = "PoopVille";
            listedTeamIdentitys.ElementAt(2).StartDate = new DateTime(1980, 3, 4);
            listedTeamIdentitys.ElementAt(2).EndDate = new DateTime(1981, 9, 5);
            teamIdentityRepository.Update(listedTeamIdentitys.ElementAt(2)).Wait();

            TeamIdentity updatedTeamIdentity = teamIdentityRepository.Get(listedTeamIdentitys.ElementAt(2).Id).Result;
            Assert.AreEqual(teams.ElementAt(1).Id, updatedTeamIdentity.TeamId);
            Assert.AreEqual("NEW TEAM ID", updatedTeamIdentity.Name);
            Assert.AreEqual("FU", updatedTeamIdentity.Abbreviation);
            Assert.AreEqual("PoopVille", updatedTeamIdentity.City);
            Assert.AreEqual(new DateTime(1980, 3, 4), updatedTeamIdentity.StartDate);
            Assert.AreEqual(new DateTime(1981, 9, 5), updatedTeamIdentity.EndDate);

            exceptionThrown = false;
            try
            {
                listedTeamIdentitys.ElementAt(3).Name = "NEW TEAM ID";
                listedTeamIdentitys.ElementAt(3).StartDate = new DateTime(1980, 3, 4);
                teamIdentityRepository.Update(listedTeamIdentitys.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedTeamIdentity = teamIdentityRepository.Get(listedTeamIdentitys.ElementAt(3).Id).Result;
            Assert.AreEqual(teams.ElementAt(0).Id, updatedTeamIdentity.TeamId);
            Assert.AreEqual("Wankers", updatedTeamIdentity.Name);
            Assert.AreEqual("Ari", updatedTeamIdentity.Abbreviation);
            Assert.AreEqual("Arizona", updatedTeamIdentity.City);
            Assert.AreEqual(new DateTime(2000,5,6), updatedTeamIdentity.StartDate);
            Assert.AreEqual(null, updatedTeamIdentity.EndDate);

            teamIdentityRepository.Delete(listedTeamIdentitys.ElementAt(0).Id).Wait();
            listedTeamIdentitys = teamIdentityRepository.List().Result;
            Assert.AreEqual(3, listedTeamIdentitys.Count());
        }
    }
}
