﻿using HtmlAgilityPack;
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
                    string name = a.InnerHtml;
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
                regularSeasonStatsTable = doc.GetElementbyId("stats_basic_plus_nhl");
                if (regularSeasonStatsTable == null)
                {
                    return null;
                }
            }

            HtmlNode tbody = regularSeasonStatsTable.Element("tbody");
            if (tbody == null)
            {
                return null;
            }

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
                    foreach(Match match in numbers.Matches(draftPosition))
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
                                        
                    // TO DO translate team to team id
                    draft.TeamName = team;
                    // TO DO translate team to team id

                    drafts.Add(draft);
                }

            }
            athlete.Drafts = drafts;

            HtmlNode uniformDiv = doc.DocumentNode.SelectNodes("//div[contains(@class, 'uni_holder')]").
                FirstOrDefault();
            if (uniformDiv != null)
            {
                List<JerseyNumber> jerseyNumbers = new List<JerseyNumber>();
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
                            string number = uniSpan.InnerHtml.Trim();                                                        
                            jerseyNumber.Number = int.Parse(number);
                            jerseyNumber.TeamName = jerseyInfo[0].Trim();
                            jerseyNumber.Years = jerseyInfo[1].Trim();
                            jerseyNumbers.Add(jerseyNumber);
                        }
                    }
                }

                athlete.JerseyNumbers = jerseyNumbers;            
            }

            if (position.ToUpper() == "G")
            {
                LoadGoalie(tbody, athlete);
                return athlete;
            }
            else
            {
                LoadSkater(tbody, athlete);
                return athlete;
            }
        }

        public Athlete LoadGoalie(HtmlNode tbody, Athlete athlete)
        {
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
                statLine.Season = season;
                statLine.Year = yearString;
                // TO DO MAP SEASON STRING TO SEADON ID

                a = tds.ElementAt(2).Element("a");
                if (a == null)
                {
                    continue;
                }

                string teamAbbreviation = a.InnerHtml;
                string teamName = a.Attributes["title"].Value;

                // TO DO MAP TEAM NAME OR ABBREV TO TEAM
                statLine.TeamAbbreviation = teamAbbreviation;
                statLine.TeamName = teamName;
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

        public Athlete LoadSkater(HtmlNode tbody, Athlete athlete)
        {
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
                statLine.Season = season;
                statLine.Year = yearString;
                // TO DO MAP SEASON STRING TO SEADON ID

                a = tds.ElementAt(2).Element("a");
                if (a == null)
                {
                    continue;
                }

                string teamAbbreviation = a.InnerHtml;
                string teamName = a.Attributes["title"].Value;

                // TO DO MAP TEAM NAME OR ABBREV TO TEAM
                statLine.TeamAbbreviation = teamAbbreviation;
                statLine.TeamName = teamName;
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
