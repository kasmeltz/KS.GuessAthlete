using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KS.GuessAthlete.Logic.Scrapers.Hockey;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;

namespace KS.GuessAthlete.Logic.Test.Scrapers.Hockey
{
    [TestClass]
    public class HockeyReferenceScraperTest
    {
        [TestMethod]
        public void TestLoadFrederickAndersen()
        {
            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            scraper.LoadAthlete(@"/players/a/anderfr01.html", "G");
        }

        [TestMethod]
        public void TestLoadAndrewAlberts()
        {
            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            scraper.LoadAthlete(@"/players/a/alberan01.html", "D");
        }

        [TestMethod]
        public void TestLoadWayneGretzky()
        {
            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            scraper.LoadAthlete(@"/players/g/gretzwa01.html", "C");
        }

        [TestMethod]
        public void TestLoadAthletesForLetterG()
        {
            int count = 0;
            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            List<Athlete> athletes = new List<Athlete>();
            foreach (var athlete in scraper.LoadAthletesForLetter('g'))
            {
                athletes.Add(athlete);
                count++;
                if (count == 4)
                {
                    break;
                }
            }

            Assert.AreEqual(4, athletes.Count);
            Assert.AreEqual("Marian Gaborik", athletes[0].Name);
            Assert.AreEqual("Kurtis Gabriel", athletes[1].Name);
            Assert.AreEqual("Bill Gadsby", athletes[2].Name);
            Assert.AreEqual("Link Gaetz", athletes[3].Name);
        }

        /*
        [TestMethod]
        public void TestLoadAthletesForLetterC()
        {
            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            List<Athlete> athletes = new List<Athlete>();
            foreach (var athlete in scraper.LoadAthletesForLetter('c'))
            {
                athletes.Add(athlete);
            }
        }
        */
    }
}