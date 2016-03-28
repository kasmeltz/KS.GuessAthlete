using HtmlAgilityPack;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;

namespace KS.GuessAthlete.Logic.Scrapers.Hockey
{
    public class HockeyReferenceScraper
    {        
        public IEnumerable<Athlete> ScrapeAthleteData()
        {
            List<Athlete> athletes = new List<Athlete>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                athletes.AddRange(LoadAthletesForLetter(c));
            }            

            return athletes;
        }

        public IEnumerable<Athlete> LoadAthletesForLetter(char letter)
        {
            List<Athlete> athletes = new List<Athlete>();

            HtmlDocument doc = new HtmlDocument();
            doc.Load("file.htm");

            return athletes;
        }

        public Athlete LoadAthlete(string url)
        {
            Athlete athlete = new Athlete();



            return athlete;
        }
    }
}
