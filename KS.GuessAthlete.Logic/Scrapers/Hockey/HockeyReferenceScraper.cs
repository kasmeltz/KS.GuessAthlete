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

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            web.UserAgent = "User - Agent:Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 49.0.2623.108 Safari / 537.36";
            web.UseCookies = true;

            string url = @"http://www.hockey-reference.com/players/" + letter.ToString().ToLower() + @"/";
            doc = web.Load(url);
            doc.Save(@"d:\dev\athletelist.txt");

            return athletes;
        }

        public Athlete LoadAthlete(string url)
        {
            Athlete athlete = new Athlete();



            return athlete;
        }
    }
}
