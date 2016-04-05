using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperDivisionRepository
    /// </summary>
    [TestClass]
    public class DapperDivisionRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void DivisionRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IDivisionRepository divisionRepository = collection.Divisions();
            IEnumerable<Division> insertedDivisions = RepositoryTestHelper.InsertDivisions();
            IEnumerable<Division> listedDivisions = divisionRepository.List().Result;

            IEnumerable<Conference> conferences = RepositoryTestHelper.InsertConferences();

            for(int i = 0;i < listedDivisions.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedDivisions.ElementAt(i), 
                        listedDivisions.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                divisionRepository.Insert(insertedDivisions.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
        
            Assert.IsTrue(exceptionThrown);

            listedDivisions.ElementAt(2).ConferenceId = conferences.ElementAt(3).Id;
            listedDivisions.ElementAt(2).Name = "Soujteast";
            listedDivisions.ElementAt(2).StartYear = 1987;
            listedDivisions.ElementAt(2).EndYear = 1996;
            divisionRepository.Update(listedDivisions.ElementAt(2)).Wait();

            Division updatedDivision = divisionRepository.Get(listedDivisions.ElementAt(2).Id).Result;
            Assert.AreEqual(conferences.ElementAt(3).Id, updatedDivision.ConferenceId);
            Assert.AreEqual("Soujteast", updatedDivision.Name);
            Assert.AreEqual(1987, updatedDivision.StartYear);
            Assert.AreEqual(1996, updatedDivision.EndYear);

            exceptionThrown = false;
            try
            {
                listedDivisions.ElementAt(3).Name = "Soujteast";
                listedDivisions.ElementAt(3).StartYear = 1987;
                divisionRepository.Update(listedDivisions.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            updatedDivision = divisionRepository.Get(listedDivisions.ElementAt(3).Id).Result;
            Assert.AreEqual(conferences.ElementAt(0).Id, updatedDivision.ConferenceId);
            Assert.AreEqual("West", updatedDivision.Name);
            Assert.AreEqual(2012, updatedDivision.StartYear);
            Assert.AreEqual(null, updatedDivision.EndYear);

            divisionRepository.Delete(listedDivisions.ElementAt(0).Id).Wait();
            listedDivisions = divisionRepository.List().Result;
            Assert.AreEqual(3, listedDivisions.Count());
        }
    }
}
