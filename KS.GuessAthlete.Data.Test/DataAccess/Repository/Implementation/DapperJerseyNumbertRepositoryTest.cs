using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperJerseyNumberRepository
    /// </summary>
    [TestClass]
    public class DapperJerseyNumberRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void JerseyNumberRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IJerseyNumberRepository jerseyNumberRepository = collection.JerseyNumbers();
            IEnumerable<JerseyNumber> insertedJerseyNumbers = RepositoryTestHelper.InsertJerseyNumbers();
            IEnumerable<JerseyNumber> listedJerseyNumbers = jerseyNumberRepository.List().Result;

            IEnumerable<Athlete> athletes = RepositoryTestHelper.InsertAthletes();
            IEnumerable<TeamIdentity> teamIdentities = RepositoryTestHelper.InsertTeamIdentities();

            for(int i = 0;i < listedJerseyNumbers.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedJerseyNumbers.ElementAt(i), 
                        listedJerseyNumbers.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                jerseyNumberRepository.Insert(insertedJerseyNumbers.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedJerseyNumbers.ElementAt(2).AthleteId = athletes.ElementAt(1).Id;
            listedJerseyNumbers.ElementAt(2).TeamIdentityId = teamIdentities.ElementAt(3).Id;
            listedJerseyNumbers.ElementAt(2).Number = 1976;
            listedJerseyNumbers.ElementAt(2).StartYear = 10;
            listedJerseyNumbers.ElementAt(2).EndYear = 506;
            jerseyNumberRepository.Update(listedJerseyNumbers.ElementAt(2)).Wait();

            JerseyNumber updatedJerseyNumber = jerseyNumberRepository.Get(listedJerseyNumbers.ElementAt(2).Id).Result;
            Assert.AreEqual(athletes.ElementAt(1).Id, updatedJerseyNumber.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(3).Id, updatedJerseyNumber.TeamIdentityId);
            Assert.AreEqual(1976, updatedJerseyNumber.Number);
            Assert.AreEqual(10, updatedJerseyNumber.StartYear);
            Assert.AreEqual(506, updatedJerseyNumber.EndYear);

            exceptionThrown = false;
            try
            {
                listedJerseyNumbers.ElementAt(3).AthleteId = athletes.ElementAt(1).Id;
                listedJerseyNumbers.ElementAt(3).TeamIdentityId = teamIdentities.ElementAt(3).Id;
                listedJerseyNumbers.ElementAt(3).Number = 1976;
                listedJerseyNumbers.ElementAt(3).StartYear = 10;
                jerseyNumberRepository.Update(listedJerseyNumbers.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedJerseyNumber = jerseyNumberRepository.Get(listedJerseyNumbers.ElementAt(3).Id).Result;
            Assert.AreEqual(athletes.ElementAt(2).Id, updatedJerseyNumber.AthleteId);
            Assert.AreEqual(teamIdentities.ElementAt(2).Id, updatedJerseyNumber.TeamIdentityId);
            Assert.AreEqual(34, updatedJerseyNumber.Number);
            Assert.AreEqual(2014, updatedJerseyNumber.StartYear);
            Assert.AreEqual(2016, updatedJerseyNumber.EndYear);

            jerseyNumberRepository.Delete(listedJerseyNumbers.ElementAt(0).Id).Wait();
            listedJerseyNumbers = jerseyNumberRepository.List().Result;
            Assert.AreEqual(3, listedJerseyNumbers.Count());

            listedJerseyNumbers = jerseyNumberRepository.ForAthlete(athletes.ElementAt(0).Id).Result;
            Assert.AreEqual(1, listedJerseyNumbers.Count());
        }
    }
}
