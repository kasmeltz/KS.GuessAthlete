using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperTeamIdentityDivisionRepository
    /// </summary>
    [TestClass]
    public class DapperTeamIdentityDivisionRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void TeamIdentityDivisionRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ITeamIdentityDivisionRepository conferenceRepository = collection.TeamIdentityDivisions();
            IEnumerable<TeamIdentityDivision> insertedTeamIdentityDivisions = RepositoryTestHelper.InsertTeamIdentityDivisions();
            IEnumerable<TeamIdentityDivision> listedTeamIdentityDivisions = conferenceRepository.List().Result;

            IEnumerable<Division> division = RepositoryTestHelper.InsertDivisions();
            IEnumerable<TeamIdentity> teamIdentities = RepositoryTestHelper.InsertTeamIdentities();

            for (int i = 0;i < listedTeamIdentityDivisions.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedTeamIdentityDivisions.ElementAt(i), 
                        listedTeamIdentityDivisions.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                conferenceRepository.Insert(insertedTeamIdentityDivisions.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedTeamIdentityDivisions.ElementAt(2).TeamIdentityId = teamIdentities.ElementAt(3).Id;
            listedTeamIdentityDivisions.ElementAt(2).DivisionId = division.ElementAt(3).Id;
            listedTeamIdentityDivisions.ElementAt(2).StartYear = 1987;
            listedTeamIdentityDivisions.ElementAt(2).EndYear = 1996;
            conferenceRepository.Update(listedTeamIdentityDivisions.ElementAt(2)).Wait();

            TeamIdentityDivision updatedTeamIdentityDivision = conferenceRepository.Get(listedTeamIdentityDivisions.ElementAt(2).Id).Result;
            Assert.AreEqual(teamIdentities.ElementAt(3).Id, updatedTeamIdentityDivision.TeamIdentityId);
            Assert.AreEqual(division.ElementAt(3).Id, updatedTeamIdentityDivision.DivisionId);
            Assert.AreEqual(1987, updatedTeamIdentityDivision.StartYear);
            Assert.AreEqual(1996, updatedTeamIdentityDivision.EndYear);

            exceptionThrown = false;
            try
            {
                listedTeamIdentityDivisions.ElementAt(3).TeamIdentityId = teamIdentities.ElementAt(3).Id;
                listedTeamIdentityDivisions.ElementAt(3).StartYear = 1987;
                conferenceRepository.Update(listedTeamIdentityDivisions.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedTeamIdentityDivision = conferenceRepository.Get(listedTeamIdentityDivisions.ElementAt(3).Id).Result;
            Assert.AreEqual(teamIdentities.ElementAt(0).Id, updatedTeamIdentityDivision.TeamIdentityId);
            Assert.AreEqual(division.ElementAt(0).Id, updatedTeamIdentityDivision.DivisionId);
            Assert.AreEqual(2012, updatedTeamIdentityDivision.StartYear);
            Assert.AreEqual(null, updatedTeamIdentityDivision.EndYear);

            conferenceRepository.Delete(listedTeamIdentityDivisions.ElementAt(0).Id).Wait();
            listedTeamIdentityDivisions = conferenceRepository.List().Result;
            Assert.AreEqual(3, listedTeamIdentityDivisions.Count());
        }
    }
}
