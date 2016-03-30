using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperDraftRepository
    /// </summary>
    [TestClass]
    public class DapperDraftRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void DraftRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IDraftRepository draftRepository = collection.Drafts();
            IEnumerable<Draft> insertedDrafts = RepositoryTestHelper.InsertDrafts();
            IEnumerable<Draft> listedDrafts = draftRepository.List().Result;

            IEnumerable<Athlete> athletes = RepositoryTestHelper.InsertAthletes();
            IEnumerable<TeamIdentity> teamIdentities = RepositoryTestHelper.InsertTeamIdentities();

            for(int i = 0;i < listedDrafts.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedDrafts.ElementAt(i), 
                        listedDrafts.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                draftRepository.Insert(insertedDrafts.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedDrafts.ElementAt(2).AthleteId = athletes.ElementAt(1).Id;
            listedDrafts.ElementAt(2).TeamIdentityId = teamIdentities.ElementAt(3).Id;
            listedDrafts.ElementAt(2).Year = 1976;
            listedDrafts.ElementAt(2).Round = 10;
            listedDrafts.ElementAt(2).Position = 506;
            draftRepository.Update(listedDrafts.ElementAt(2)).Wait();

            Draft updatedDraft = draftRepository.Get(listedDrafts.ElementAt(2).Id).Result;
            Assert.AreEqual(athletes.ElementAt(1).Id, updatedDraft.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(3).Id, updatedDraft.TeamIdentityId);
            Assert.AreEqual(1976, updatedDraft.Year);
            Assert.AreEqual(10, updatedDraft.Round);
            Assert.AreEqual(506, updatedDraft.Position);

            exceptionThrown = false;
            try
            {
                listedDrafts.ElementAt(3).AthleteId = athletes.ElementAt(1).Id;
                listedDrafts.ElementAt(3).Year = 1976;
                draftRepository.Update(listedDrafts.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedDraft = draftRepository.Get(listedDrafts.ElementAt(3).Id).Result;
            Assert.AreEqual(athletes.ElementAt(2).Id, updatedDraft.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(2).Id, updatedDraft.TeamIdentityId);
            Assert.AreEqual(2015, updatedDraft.Year);
            Assert.AreEqual(7, updatedDraft.Round);
            Assert.AreEqual(202, updatedDraft.Position);

            draftRepository.Delete(listedDrafts.ElementAt(0).Id).Wait();
            listedDrafts = draftRepository.List().Result;
            Assert.AreEqual(3, listedDrafts.Count());
        }
    }
}
