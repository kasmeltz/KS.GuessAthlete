using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KS.GuessAthlete.Logic.Scrapers.Hockey;

namespace KS.GuessAthlete.Logic.Test.Scrapers.Hockey
{
    [TestClass]
    public class HockeyReferenceScraperTest
    {
        [TestMethod]
        public void TestScrapeAthleteData()
        {
            HockeyReferenceScraper scraper = new HockeyReferenceScraper();
            scraper.ScrapeAthleteData();
        }
    }
}