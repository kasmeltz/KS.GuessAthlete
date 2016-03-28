using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KS.GuessAthlete.Logic.Scrapers.Hockey;

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

    }
}