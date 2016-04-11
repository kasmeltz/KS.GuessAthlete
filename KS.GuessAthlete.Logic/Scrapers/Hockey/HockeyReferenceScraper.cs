using HtmlAgilityPack;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Data.POCO.Hockey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KS.GuessAthlete.Logic.Scrapers.Hockey
{
    public class HockeyReferenceScraper
    {
        public const string BASE_URL = "http://www.hockey-reference.com";

        public IEnumerable<Athlete> ScrapeAthleteData(IEnumerable<Athlete> existingAthletes = null)
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                IEnumerable<Athlete> athletes = LoadAthletesForLetter(c, existingAthletes);
                foreach (Athlete athlete in athletes)
                {
                    yield return athlete;
                }
            }
        }

        public IEnumerable<Athlete> LoadAthletesForLetter(char letter, IEnumerable<Athlete> existingAthletes = null, bool skipExisting = true)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            web.UserAgent = @"User - Agent:Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 49.0.2623.108 Safari / 537.36";
            web.UseCookies = true;

            string url = BASE_URL + @"/players/" + letter.ToString().ToLower() + @"/";
            doc = web.Load(url);

            HtmlNode playersTable = doc.GetElementbyId("players");
            if (playersTable == null)
            {
                yield return null;
            }

            HtmlNode tbody = playersTable.Element("tbody");
            foreach (HtmlNode row in tbody.Elements("tr"))
            {
                string cssClass = row.Attributes["class"].Value;
                if (cssClass == "nhl")
                {
                    IEnumerable<HtmlNode> tds = row.Elements("td");
                    HtmlNode a = tds.ElementAt(0)
                        .Descendants()
                        .Where(nod => nod.Attributes["href"] != null)
                        .FirstOrDefault();
                    if (a == null)
                    {
                        continue;
                    }
                    string href = a.Attributes["href"].Value;
                    string name = a.InnerHtml;
                                    
                    if (existingAthletes != null && skipExisting)
                    {
                        Athlete existingAthlete = existingAthletes
                            .Where(ath => ath.Name == name).FirstOrDefault();

                        if (existingAthlete != null)
                        {
                            continue;
                        }
                    }

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

                    athlete.Name = name;
                    athlete.BirthDate = birthDate;
                    athlete.Height = height;
                    athlete.Weight = weight;
                    athlete.Position = position;

                    yield return athlete;
                }
            }
        }

        public Athlete LoadAthlete(string url, string position)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            web.UserAgent = @"User - Agent:Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 49.0.2623.108 Safari / 537.36";
            web.UseCookies = true;

            doc = web.Load(BASE_URL + url);

            Athlete athlete = new Athlete();

            HtmlNode necroBirthSpan = doc.GetElementbyId("necro-birth");
            if (necroBirthSpan == null)
            {
                return null;
            }
            HtmlNode p = necroBirthSpan.ParentNode;
            string innerText = p.InnerText;
            int inIndex = innerText.IndexOf(" in ");
            if (inIndex >= 0)
            {
                string birthPlace = innerText.Substring(inIndex + 4);
                int slashIndex = birthPlace.IndexOf("\n");
                birthPlace = birthPlace.Substring(0, slashIndex).Trim();
                string[] birthInfos = birthPlace.Split(',');
                if (birthInfos.Length > 1)
                {
                    athlete.BirthCity = birthInfos[0].Trim();
                    athlete.BirthCountry = birthInfos[1].Trim();
                }
            }

            List<Draft> drafts = new List<Draft>();
            int draftIndex = innerText.IndexOf("Draft: ");
            if (draftIndex >= 0)
            {
                string draftInfo = innerText.Substring(draftIndex + 7);
                string[] draftInfos = draftInfo
                    .Split(new string[] { @"&amp;" },
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (string draftDetails in draftInfos)
                {
                    int slashIndex = draftDetails.IndexOf("\n");
                    string betterDraftDetails = draftDetails.Trim();
                    if (slashIndex >= 0)
                    {
                        betterDraftDetails = draftDetails.Substring(0, slashIndex).Trim();
                    }
                    string[] draftPieces = betterDraftDetails.Split(',');

                    string team = draftPieces[0].Trim();
                    string year = draftPieces[2].Trim().Substring(0, 4);
                    string draftPosition = draftPieces[1].Trim();
                    string draftRound = draftPosition.Substring(0, 1);
                    int bracketIndex = draftPosition.IndexOf("(");
                    draftPosition = draftPosition.Substring(bracketIndex + 1);
                    Regex numbers = new Regex("[0-9]+");
                    foreach (Match match in numbers.Matches(draftPosition))
                    {
                        draftPosition = match.Value;
                        break;
                    }

                    int i;
                    Draft draft = new Draft();

                    int.TryParse(draftPosition, out i);
                    draft.Position = i;
                    int.TryParse(draftRound, out i);
                    draft.Round = i;
                    int.TryParse(year, out i);
                    draft.Year = i;

                    draft.TeamName = team;

                    drafts.Add(draft);
                }

            }
            athlete.Drafts = drafts;

            IEnumerable<HtmlNode> uniformDivs = doc.DocumentNode
                .SelectNodes("//div[contains(@class, 'uni_holder')]");

            HtmlNode uniformDiv = null;
            if (uniformDivs != null)
            {
                uniformDiv = uniformDivs
                    .FirstOrDefault();
            }

            List<JerseyNumber> jerseyNumbers = new List<JerseyNumber>();
            if (uniformDiv != null)
            {                
                IEnumerable<HtmlNode> uniSpans = uniformDiv.SelectNodes("//span[contains(@class, 'uni_square')]");
                foreach (HtmlNode uniSpan in uniSpans)
                {
                    if (uniSpan.Attributes["tip"] != null)
                    {
                        string teamString = uniSpan.Attributes["tip"].Value.Trim();
                        string[] jerseyInfo = teamString.Split(',');
                        if (jerseyInfo.Length == 2)
                        {
                            JerseyNumber jerseyNumber = new JerseyNumber();
                            Regex numbers = new Regex("[0-9]+");
                            string number = "";
                            foreach (Match match in numbers.Matches(uniSpan.InnerHtml.Trim()))
                            {
                                number = match.Value;
                                break;
                            }
                            jerseyNumber.Number = int.Parse(number);
                            jerseyNumber.TeamName = jerseyInfo[0].Trim();
                            jerseyNumber.Years = jerseyInfo[1].Trim();

                            string[] years = jerseyNumber.Years.Split('-');
                            if (years.Length > 0)
                            {
                                jerseyNumber.StartYear = int.Parse(years[0]);
                            }
                            if (years.Length > 1)
                            {
                                jerseyNumber.EndYear = int.Parse(years[1]);
                            }

                            jerseyNumbers.Add(jerseyNumber);
                        }
                    }
                }
            }
            athlete.JerseyNumbers = jerseyNumbers;


            LoadStats(athlete, position, doc, "stats_basic_nhl", 0);
            LoadStats(athlete, position, doc, "stats_basic_plus_nhl", 0);
            LoadStats(athlete, position, doc, "stats_playoffs_nhl", 1);

            return athlete;
        }

        public void LoadStats(Athlete athlete, string position, HtmlDocument doc, string divId, int isPlayoffs)
        {
            HtmlNode regularSeasonStatsTable = doc.GetElementbyId(divId);
            if (regularSeasonStatsTable == null)
            {
                return;
            }
            HtmlNode tbody = regularSeasonStatsTable.Element("tbody");
            if (tbody == null)
            {
                return;
            }

            if (position.ToUpper() == "G")
            {
                LoadGoalieStats(athlete, isPlayoffs, tbody);
            }
            else
            {
                LoadSkaterStats(athlete, isPlayoffs, tbody);
            }
        }

        public string InnerHtmlToString(HtmlNode node)
        {
            string text = node.InnerHtml;
            text = text.Replace("<b>", "").Replace("</b>", "");
            text = text.Replace("<strong>", "").Replace("</strong>", "");
            return text;
        }

        public int InnerHtmlToInt(HtmlNode node)
        {
            int i;
            string text = InnerHtmlToString(node);
            int.TryParse(text, out i);
            return i;
        }

        public decimal InnerHtmlToDecimal(HtmlNode node)
        {
            decimal d;
            string text = InnerHtmlToString(node);
            decimal.TryParse(text, out d);
            return d;
        }

        public void LoadGoalieStats(Athlete athlete, int isPlayoffs, HtmlNode tbody)
        {
            List<StatLine> statsLines = new List<StatLine>();

            if (tbody == null)
            {
                athlete.Stats = statsLines;
                return;
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

                statLine.Season = season;
                statLine.Year = year;

                a = tds.ElementAt(2).Element("a");
                if (a == null)
                {
                    continue;
                }

                string teamAbbreviation = a.InnerHtml;
                string teamName = a.Attributes["title"].Value;

                statLine.TeamAbbreviation = teamAbbreviation;
                statLine.TeamName = teamName;

                int currentTD = 4;

                if (isPlayoffs == 1)
                {
                    if (string.IsNullOrEmpty(tds.ElementAt(currentTD++).InnerHtml))
                    {
                        statLine.StanleyCup = 0;
                    }
                    else
                    {
                        statLine.StanleyCup = 1;
                    }
                }

                statLine.GamesPlayed = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.GamesStarted = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Wins = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Losses = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.TiesPlusOvertimeShootoutLosses = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.GoalsAgainst = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.ShotsAgainst = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Saves = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.SavePercentage = InnerHtmlToDecimal(tds.ElementAt(currentTD++));
                statLine.GoalsAgainstAverage = InnerHtmlToDecimal(tds.ElementAt(currentTD++));
                statLine.Shutouts = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Minutes = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.QualityStarts = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.QualityStartPercentage = InnerHtmlToDecimal(tds.ElementAt(currentTD++));
                statLine.ReallyBadStarts = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.GoalsAgainstPercentage = InnerHtmlToDecimal(tds.ElementAt(currentTD++));
                statLine.GoalsSavedAboveAverage = InnerHtmlToDecimal(tds.ElementAt(currentTD++));

                if (isPlayoffs == 0)
                {
                    statLine.GoaliePointShares = InnerHtmlToDecimal(tds.ElementAt(currentTD++));
                }

                statLine.Goals = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Assists = InnerHtmlToInt(tds.ElementAt(currentTD++));

                // skip points column
                currentTD++;

                statLine.PenaltyMinutes = InnerHtmlToInt(tds.ElementAt(currentTD++));

                if (isPlayoffs == 0)
                {
                    IEnumerable<HtmlNode> awardTags = tds.ElementAt(currentTD++).Elements("a");
                    foreach (HtmlNode node in awardTags)
                    {
                        statLine.Awards += "@" + InnerHtmlToString(node) + "@";
                    }
                }

                statLine.IsPlayoffs = isPlayoffs;
                statsLines.Add(statLine);
            }

            if (athlete.Stats != null)
            {
                statsLines.AddRange(athlete.Stats);
            }
            athlete.Stats = statsLines;
        }

        public void LoadSkaterStats(Athlete athlete, int isPlayoffs, HtmlNode tbody)
        {
            List<StatLine> statsLines = new List<StatLine>();

            if (tbody == null)
            {
                athlete.Stats = statsLines;
                return;
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

                statLine.Season = season;
                statLine.Year = year;

                a = tds.ElementAt(2).Element("a");
                if (a == null)
                {
                    continue;
                }

                string teamAbbreviation = a.InnerHtml;
                string teamName = a.Attributes["title"].Value;

                statLine.TeamAbbreviation = teamAbbreviation;
                statLine.TeamName = teamName;

                int i;
                decimal d;
                int currentTD = 4;

                if (isPlayoffs == 1)
                {
                    if (string.IsNullOrEmpty(tds.ElementAt(currentTD++).InnerHtml))
                    {
                        statLine.StanleyCup = 0;
                    }
                    else
                    {
                        statLine.StanleyCup = 1;
                    }
                }

                statLine.GamesPlayed = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Goals = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.Assists = InnerHtmlToInt(tds.ElementAt(currentTD++));

                // skip points column
                currentTD++;

                statLine.PlusMinus = InnerHtmlToInt(tds.ElementAt(currentTD++)); 
                statLine.PenaltyMinutes = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.EvenStrengthGoals = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.PowerPlayGoals = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.ShortHandedGoals = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.GameWinningGoals = InnerHtmlToInt(tds.ElementAt(currentTD++));

                if (isPlayoffs == 0)
                {
                    statLine.EvenStrengthAssists = InnerHtmlToInt(tds.ElementAt(currentTD++));
                    statLine.PowerPlayAssists = InnerHtmlToInt(tds.ElementAt(currentTD++));
                    statLine.ShortHandedAssists = InnerHtmlToInt(tds.ElementAt(currentTD++));
                }

                statLine.Shots = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.ShotPercentage = InnerHtmlToDecimal(tds.ElementAt(currentTD++));
                statLine.TimeOnIce = InnerHtmlToInt(tds.ElementAt(currentTD++));
                statLine.AverageTimeOnIce = InnerHtmlToDecimal(tds.ElementAt(currentTD++));

                if (isPlayoffs == 0)
                {
                    IEnumerable<HtmlNode> awardTags = tds.ElementAt(currentTD++).Elements("a");
                    foreach (HtmlNode node in awardTags)
                    {
                        statLine.Awards += "@" + InnerHtmlToString(node) + "@";
                    }
                }

                statLine.IsPlayoffs = isPlayoffs;
                statsLines.Add(statLine);
            }

            if (athlete.Stats != null)
            {
                statsLines.AddRange(athlete.Stats);
            }
            athlete.Stats = statsLines;
        }
    }
}
