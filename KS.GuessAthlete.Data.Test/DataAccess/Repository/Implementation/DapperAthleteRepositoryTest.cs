using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperAthleteRepository
    /// </summary>
    [TestClass]
    public class DapperAthleteRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void AthleteRepository()
        {
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            IAthleteRepository leagueRepository = collection.Athletes();
            IEnumerable<Athlete> insertedAthletes = RepositoryTestHelper.InsertAthletes();
            IEnumerable<Athlete> listedAthletes = leagueRepository.List().Result;

            for(int i = 0;i < listedAthletes.Count();i++)
            {
                RepositoryTestHelper
                    .AssertProperties(insertedAthletes.ElementAt(i), 
                        listedAthletes.ElementAt(i));
            }

            bool exceptionThrown = false;
            try
            {
                // Act
                leagueRepository.Insert(insertedAthletes.ElementAt(1)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);

            listedAthletes.ElementAt(2).Name = "New Athlete Name";
            listedAthletes.ElementAt(2).BirthDate = new DateTime(1902, 1, 1);
            listedAthletes.ElementAt(2).BirthCity = "Somewhere";
            listedAthletes.ElementAt(2).BirthCountry = "Country";
            listedAthletes.ElementAt(2).Position = "Doggy";
            listedAthletes.ElementAt(2).Height = "4'5";
            listedAthletes.ElementAt(2).Weight = "500";
            leagueRepository.Update(listedAthletes.ElementAt(2)).Wait();

            Athlete updatedAthlete = leagueRepository.Get(listedAthletes.ElementAt(2).Id).Result;
            Assert.AreEqual("New Athlete Name", updatedAthlete.Name);
            Assert.AreEqual(new DateTime(1902, 1, 1), updatedAthlete.BirthDate);
            Assert.AreEqual("Somewhere", updatedAthlete.BirthCity);
            Assert.AreEqual("Country", updatedAthlete.BirthCountry);
            Assert.AreEqual("Doggy", updatedAthlete.Position);
            Assert.AreEqual("4'5", updatedAthlete.Height);
            Assert.AreEqual("500", updatedAthlete.Weight);

            exceptionThrown = false;
            try
            {
                listedAthletes.ElementAt(3).Name = "New Athlete Name";
                listedAthletes.ElementAt(3).BirthDate = new DateTime(1902, 1, 1);
                leagueRepository.Update(listedAthletes.ElementAt(3)).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            updatedAthlete = leagueRepository.Get(listedAthletes.ElementAt(3).Id).Result;
            Assert.AreEqual("Wayne Gretzky", updatedAthlete.Name);            

            leagueRepository.Delete(listedAthletes.ElementAt(0).Id);
            listedAthletes = leagueRepository.List().Result;
            Assert.AreEqual(3, listedAthletes.Count());
        }
    }
}
