using HtmlAgilityPack;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;

namespace KS.GuessAthlete.Logic.Scrapers.Hockey
{
    public class HockeyReferenceScraper
    {
        public const string BASE_URL = "http://www.hockey-reference.com";

        public IEnumerable<Athlete> ScrapeAthleteData()
        {
            List<Athlete> athletes = new List<Athlete>();

            for (char c = 'A'; c <= 'A'; c++)
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

            web.UserAgent = @"User - Agent:Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 49.0.2623.108 Safari / 537.36";
            web.UseCookies = true;

            string url = BASE_URL + @"/players/" + letter.ToString().ToLower() + @"/";
            doc = web.Load(url);

            HtmlNode playersTable = doc.GetElementbyId("players");
            if (playersTable == null)
            {
                return athletes;
            }

            HtmlNode tbody = playersTable.Element("tbody");
            foreach (HtmlNode row in tbody.Elements("tr"))
            {
                string cssClass = row.Attributes["class"].Value;
                if (cssClass == "nhl")
                {
                    HtmlNode td = row.Element("td");
                    if (td == null)
                    {
                        continue;
                    }
                    HtmlNode a = td.Element("a");
                    if (a == null)
                    {
                        continue;
                    }
                    string href = a.Attributes["href"].Value;
                    Athlete athlete = LoadAthlete(href);
                    if (athlete == null )
                    {
                        continue;
                    }

                    athletes.Add(athlete);
                    athlete.Name = a.InnerHtml;
                    
                }
            }

            return athletes;
        }

        public Athlete LoadAthlete(string url)
        {
            Athlete athlete = new Athlete();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            web.UserAgent = @"User - Agent:Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 49.0.2623.108 Safari / 537.36";
            web.UseCookies = true;

            doc = web.Load(BASE_URL + url);

            HtmlNode regularSeasonStatsTable = doc.GetElementbyId("stats_basic_nhl");
            HtmlNode tbody = regularSeasonStatsTable.Element("tbody");
            if (tbody == null)
            {
                return null;
            }

            foreach (HtmlNode row in tbody.Elements("tr"))
            {
                IEnumerable<HtmlNode> tds = row.Elements("tr");
            }


            return athlete;
        }
    }
}
