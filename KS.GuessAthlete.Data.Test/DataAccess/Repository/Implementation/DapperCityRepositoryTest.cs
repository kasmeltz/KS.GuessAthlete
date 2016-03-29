using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KS.SportsOps.Data.Test.DataAccess.Repository.Implementation
{
    /// <summary>
    /// Unit Tests for the DapperCityRepository
    /// </summary>
    [TestClass]
    public class DapperCityRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            RepositoryTestHelper.PrepareForTest();
        }

        [TestMethod]
        public void CityRepository()
        {
            IEnumerable<League> leagues = RepositoryTestHelper.InsertLeagues();
            IRepositoryCollection collection = RepositoryTestHelper.Collection();
            ILeagueRepository leagueRepository = collection.Leagues();

            /*

            IEnumerable<City> cities = null;
            IEnumerable<City> citySearch = null;
            City city1 = null;
            City city2 = null;
            City city3 = null;
            City city4 = null;
            City city5 = null;
            City getCity1 = null;
            City getCity2 = null;
            City getCity3 = null;
            City getCity4 = null;
            bool exceptionThrown = false;

            city1 = new City { StateId = state1.Id, Name = "Thunder Bay", Abbreviation = "TBY" };
            city2 = new City { StateId = state1.Id, Name = "Toronto", Abbreviation = "TOR" };
            city3 = new City { StateId = state2.Id, Name = "Edmonton", Abbreviation = "EDM" };
            city4 = new City { StateId = state3.Id, Name = "Moscow", Abbreviation = "MOS" };

            // Act
            cityRepository.Insert(city1).Wait();
            cityRepository.Insert(city2).Wait();
            cityRepository.Insert(city3).Wait();
            cityRepository.Insert(city4).Wait();

            // Assert
            getCity1 = cityRepository.Get(city1.Id).Result;
            RepositoryTestHelper.AssertProperties(city1, getCity1);

            getCity2 = cityRepository.Get(city2.Id).Result;
            RepositoryTestHelper.AssertProperties(city2, getCity2);

            getCity3 = cityRepository.Get(city3.Id).Result;
            RepositoryTestHelper.AssertProperties(city3, getCity3);

            getCity4 = cityRepository.Get(city4.Id).Result;
            RepositoryTestHelper.AssertProperties(city4, getCity4);

            exceptionThrown = false;
            try
            {
                // Act
                city5 = new City { StateId = state1.Id, Name = "Thunder Bay", Abbreviation = "TBY" };
                cityRepository.Insert(city5).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);

            // Act            
            cities = cityRepository.List().Result;
            Assert.AreEqual(4, cities.Count());

            // Act           
            cityRepository.Delete(city1.Id).Wait();
            cities = cityRepository.List().Result;
            getCity1 = cityRepository.Get(city1.Id).Result;

            // Assert
            Assert.AreEqual(3, cities.Count());
            Assert.IsNull(getCity1);

            // Act 
            city2.Name = "Ottawa";
            city2.Abbreviation = "OTT";
            cityRepository.Update(city2).Wait();
            getCity2 = cityRepository.Get(city2.Id).Result;

            // Assert
            Assert.AreEqual("Ottawa", getCity2.Name);
            Assert.AreEqual("OTT", getCity2.Abbreviation);

            exceptionThrown = false;
            try
            {
                // Act
                city3.Name = "Ottawa";
                city3.StateId = state1.Id;
                cityRepository.Update(city3).Wait();
            }
            catch (AggregateException)
            {
                exceptionThrown = true;
            }
            getCity3 = cityRepository.Get(city3.Id).Result;

            // Assert
            Assert.IsTrue(exceptionThrown);
            Assert.AreEqual("Edmonton", getCity3.Name);

            // Act            
            citySearch = cityRepository.Search("awa").Result;

            // Assert
            Assert.AreEqual(1, citySearch.Count());
            Assert.AreEqual(city2.Id, citySearch.ElementAt(0).Id);
            Assert.AreEqual("Ottawa", citySearch.ElementAt(0).Name);
            Assert.AreEqual("OTT", citySearch.ElementAt(0).Abbreviation);

            // Act            
            cityRepository.Delete(city2.Id).Wait();
            cityRepository.Delete(city3.Id).Wait();
            cityRepository.Delete(city4.Id).Wait();
            cities = cityRepository.List().Result;

            // Assert
            Assert.AreEqual(0, cities.Count());
            */
        }
    }
}
