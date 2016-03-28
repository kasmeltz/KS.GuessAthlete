using HtmlAgilityPack;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    IEnumerable<HtmlNode> tds = row.Elements("td");
                    HtmlNode a = tds.ElementAt(0).Element("a");
                    if (a == null)
                    {
                        continue;
                    }
                    string href = a.Attributes["href"].Value;
                    string position = tds.ElementAt(3).InnerHtml;
                    string height = tds.ElementAt(4).InnerHtml;
                    string weight = tds.ElementAt(5).InnerHtml;
                    a = tds.ElementAt(6).Element("a");
                    DateTime birthDate = DateTime.Now;
                    if (a != null)
                    {
                        DateTime.TryParse(a.InnerHtml, out birthDate);
                    }
                    Athlete athlete = LoadAthlete(href, position);
                    if (athlete == null)
                    {
                        continue;
                    }

                    athlete.Name = a.InnerHtml;
                    athlete.BirthDate = birthDate;
                    athlete.Height = height;
                    athlete.Weight = weight;
                    athlete.Position = position;
                    athletes.Add(athlete);
                }
            }

            return athletes;
        }

        public Athlete LoadAthlete(string url, string position)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            web.UserAgent = @"User - Agent:Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 49.0.2623.108 Safari / 537.36";
            web.UseCookies = true;

            doc = web.Load(BASE_URL + url);

            HtmlNode regularSeasonStatsTable = doc.GetElementbyId("stats_basic_nhl");
            if (regularSeasonStatsTable == null)
            {
                return null;
            }

            HtmlNode tbody = regularSeasonStatsTable.Element("tbody");
            if (tbody == null)
            {
                return null;
            }

            if (position.ToUpper() == "G")
            {
                return LoadGoalie(tbody);
            }
            else
            {
                return LoadSkater(tbody);
            }
        }

        public Athlete LoadGoalie(HtmlNode tbody)
        {
            Athlete athlete = new Athlete();
            List<StatLine> statsLines = new List<StatLine>();

            if (tbody == null)
            {
                return null;
            }

            foreach (HtmlNode row in tbody.Elements("tr"))
            {
                GoalieStatLine statLine = new GoalieStatLine();
                IEnumerable<HtmlNode> tds = row.Elements("td");
                HtmlNode a = tds.ElementAt(3).Element("a");
                if (a.InnerHtml.ToUpper() != "NHL")
                {
                    continue;
                }

                string season = tds.ElementAt(0).InnerHtml;
                string yearString = season.Trim().Substring(0, 4);
                int year;
                if (!int.TryParse(yearString, out year))
                {
                    continue;
                }

                // TO DO MAP SEASON STRING TO SEADON ID
                statLine.SeasonId = 1;
                // TO DO MAP SEASON STRING TO SEADON ID

                a = tds.ElementAt(2).Element("a");
                if (a == null)
                {
                    continue;
                }

                string teamAbbreviation = a.InnerHtml;
                string teamName = a.Attributes["title"].Value;

                // TO DO MAP TEAM NAME OR ABBREV TO TEAM
                statLine.TeamId = 1;
                // TO DO MAP TEAM NAME OR ABBREV TO TEAM

                int i;
                decimal d;
                int.TryParse(tds.ElementAt(4).InnerHtml, out i);
                statLine.GamesPlayed = i;
                int.TryParse(tds.ElementAt(5).InnerHtml, out i);
                statLine.GamesStarted = i;
                int.TryParse(tds.ElementAt(6).InnerHtml, out i);
                statLine.Wins = i;
                int.TryParse(tds.ElementAt(7).InnerHtml, out i);
                statLine.Losses = i;
                int.TryParse(tds.ElementAt(8).InnerHtml, out i);
                statLine.TiesPlusOvertimeShootoutLosses = i;
                int.TryParse(tds.ElementAt(9).InnerHtml, out i);
                statLine.GoalsAgainst = i;
                int.TryParse(tds.ElementAt(10).InnerHtml, out i);
                statLine.ShotsAgainst = i;
                int.TryParse(tds.ElementAt(11).InnerHtml, out i);
                statLine.Saves = i;
                decimal.TryParse(tds.ElementAt(12).InnerHtml, out d);
                statLine.SavePercentage = d;
                decimal.TryParse(tds.ElementAt(13).InnerHtml, out d);
                statLine.GoalsAgainstAverage = d;
                int.TryParse(tds.ElementAt(14).InnerHtml, out i);
                statLine.Shutouts = i;
                int.TryParse(tds.ElementAt(15).InnerHtml, out i);
                statLine.Minutes = i;
                int.TryParse(tds.ElementAt(16).InnerHtml, out i);
                statLine.QualityStarts = i;
                decimal.TryParse(tds.ElementAt(17).InnerHtml, out d);
                statLine.QualityStartPercentage = d;
                int.TryParse(tds.ElementAt(18).InnerHtml, out i);
                statLine.ReallyBadStarts = i;
                decimal.TryParse(tds.ElementAt(19).InnerHtml, out d);
                statLine.GoalsAgainstPercentage = i;
                decimal.TryParse(tds.ElementAt(20).InnerHtml, out d);
                statLine.GoalsSavedAboveAverage = i;
                decimal.TryParse(tds.ElementAt(21).InnerHtml, out d);
                statLine.GoaliePointShares = i;
                int.TryParse(tds.ElementAt(22).InnerHtml, out i);
                statLine.Goals = i;
                int.TryParse(tds.ElementAt(23).InnerHtml, out i);
                statLine.Assists = i;
                int.TryParse(tds.ElementAt(25).InnerHtml, out i);
                statLine.PenaltyMinutes = i;

                IEnumerable<HtmlNode> awardTags = tds.ElementAt(26).Elements("a");
                foreach (HtmlNode node in awardTags)
                {
                    statLine.Awards += "@" + node.InnerHtml + "@";
                }

                statsLines.Add(statLine);
            }

            athlete.Stats = statsLines;

            return athlete;
        }

        public Athlete LoadSkater(HtmlNode tbody)
        {
            Athlete athlete = new Athlete();
            List<StatLine> statsLines = new List<StatLine>();

            if (tbody == null)
            {
                return null;
            }

            foreach (HtmlNode row in tbody.Elements("tr"))
            {
                SkaterStatLine statLine = new SkaterStatLine();

                IEnumerable<HtmlNode> tds = row.Elements("td");
                HtmlNode a = tds.ElementAt(3).Element("a");
                if (a.InnerHtml.ToUpper() != "NHL")
                {
                    continue;
                }

                string season = tds.ElementAt(0).InnerHtml;
                string yearString = season.Trim().Substring(0, 4);
                int year;
                if (!int.TryParse(yearString, out year))
                {
                    continue;
                }

                // TO DO MAP SEASON STRING TO SEADON ID
                statLine.SeasonId = 1;
                // TO DO MAP SEASON STRING TO SEADON ID

                a = tds.ElementAt(2).Element("a");
                if (a == null)
                {
                    continue;
                }

                string teamAbbreviation = a.InnerHtml;
                string teamName = a.Attributes["title"].Value;

                // TO DO MAP TEAM NAME OR ABBREV TO TEAM
                statLine.TeamId = 1;
                // TO DO MAP TEAM NAME OR ABBREV TO TEAM

                int i;
                decimal d;
                int.TryParse(tds.ElementAt(4).InnerHtml, out i);
                statLine.GamesPlayed = i;
                int.TryParse(tds.ElementAt(5).InnerHtml, out i);
                statLine.Goals = i;
                int.TryParse(tds.ElementAt(6).InnerHtml, out i);
                statLine.Assists = i;
                int.TryParse(tds.ElementAt(8).InnerHtml, out i);
                statLine.PlusMinus = i;
                int.TryParse(tds.ElementAt(9).InnerHtml, out i);
                statLine.PenaltyMinutes = i;
                int.TryParse(tds.ElementAt(10).InnerHtml, out i);
                statLine.EvenStrengthGoals = i;
                int.TryParse(tds.ElementAt(11).InnerHtml, out i);
                statLine.PowerPlayGoals = i;
                int.TryParse(tds.ElementAt(12).InnerHtml, out i);
                statLine.ShortHandedGoals = i;
                int.TryParse(tds.ElementAt(13).InnerHtml, out i);
                statLine.GameWinningGoals = i;
                int.TryParse(tds.ElementAt(14).InnerHtml, out i);
                statLine.EvenStrengthAssists = i;
                int.TryParse(tds.ElementAt(15).InnerHtml, out i);
                statLine.PowerPlayAssists = i;
                int.TryParse(tds.ElementAt(16).InnerHtml, out i);
                statLine.ShortHandedAssists = i;
                int.TryParse(tds.ElementAt(17).InnerHtml, out i);
                statLine.Shots = i;
                decimal.TryParse(tds.ElementAt(18).InnerHtml, out d);
                statLine.ShotPercentage = i;
                int.TryParse(tds.ElementAt(19).InnerHtml, out i);
                statLine.TimeOnIce = i;
                decimal.TryParse(tds.ElementAt(20).InnerHtml, out d);
                statLine.AverageTimeOnIce = i;

                IEnumerable<HtmlNode> awardTags = tds.ElementAt(21).Elements("a");
                foreach (HtmlNode node in awardTags)
                {
                    statLine.Awards += "@" + node.InnerHtml + "@";
                }

                statsLines.Add(statLine);
            }

            athlete.Stats = statsLines;

            return athlete;
        }
    }
}
