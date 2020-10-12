using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using WebCrawlerTest.Entities;

namespace WebCrawlerTest.Scrappers
{
    public class BetScrapper
    {
        public static List<Bet> GetMainMarketBets(IWebDriver driver, string champ, string teamA, string teamB)
        {
            var bets = new List<Bet>();
            
            var collapsablePanels = driver.WaitToFindElements(TimeSpan.FromSeconds(5), By.ClassName("collapsablePanel"));

            foreach (var cp in collapsablePanels)
            {
                var type = cp.FindElement(By.ClassName("title")).Text;
                var odds = cp.FindElements(By.ClassName("odds"));

                // Vencedor da Partida
                if (type.StartsWith("Vencedor da Partida"))
                {
                    var bet1 = new Bet
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet1);

                    var bet2 = new Bet
                    {
                        Type = type,
                        Team = teamB,
                        CounterTeam = teamA,
                        Champ = champ,
                        Odd = float.Parse(odds[1].Text)
                    };

                    bets.Add(bet2);
                }
                
                // Mapas Handicap
                else if (type.StartsWith("Mapas Handicap"))
                {

                    var handicaps = cp.FindElements(By.ClassName("handicap"));

                    var bet1 = new Bet
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = handicaps[1].Text,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet1);

                    var bet2 = new Bet
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = handicaps[2].Text,
                        Odd = float.Parse(odds[1].Text)
                    };

                    bets.Add(bet2);

                }
                
                // Total de Mapas
                else if (type.StartsWith("Total de Mapas"))
                {

                    var handicaps = cp.FindElements(By.ClassName("handicap"));
                    var outcomeHeaders = cp.FindElements(By.ClassName("outcomeHeader"));

                    var bet1 = new Bet()
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = outcomeHeaders[0].Text + " " + handicaps[1].Text,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet1);

                    var bet2 = new Bet()
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = outcomeHeaders[1].Text + " " + handicaps[2].Text,
                        Odd = float.Parse(odds[1].Text)
                    };

                    bets.Add(bet2);

                }
                
                // Resultado Exato
                else if (type.StartsWith("Resultado Exato"))
                {
                    var outcomeEntries = cp.FindElements(By.ClassName("outcomeEntry"));

                    foreach (var oe in outcomeEntries)
                    {

                        var outcomeItemHeader = oe.FindElement(By.ClassName("outcomeItemHeader"));

                        var team = outcomeItemHeader.Text.Split(" ")[0];

                        var bet = new Bet
                        {
                            Type = type,
                            Team = team,
                            CounterTeam = string.Equals(team, teamA) ? teamB : teamA,
                            Champ = champ,
                            Detail = outcomeItemHeader.Text.Split(" ")[1],
                            Odd = float.Parse(odds[0].Text)
                        };

                        bets.Add(bet);

                    }

                }
                
                // Para ganhar ao menos um mapa
                else if (type.EndsWith("Para ganhar ao menos um mapa"))
                {

                    var bet = new Bet
                    {
                        Type = type,
                        Team = type.StartsWith("Equipe A") ? teamA : teamB,
                        CounterTeam = type.StartsWith("Equipe A") ? teamB : teamA,
                        Champ = champ,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet);

                }

            }

            return bets;
        }

        public static List<Bet> GetMapMarketBets(IWebDriver driver, string market, string champ, string teamA, string teamB)
        {
            var bets = new List<Bet>();

            var collapsablePanels = driver.WaitToFindElements(TimeSpan.FromSeconds(5), By.ClassName("collapsablePanel"));

            foreach (var cp in collapsablePanels)
            {
                var type = cp.FindElement(By.ClassName("title")).Text;
                var odds = cp.FindElements(By.ClassName("odds"));

                // Vencedor do Mapa
                if (type.StartsWith("Vencedor do Mapa"))
                {
                    var bet1 = new Bet
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet1);

                    var bet2 = new Bet
                    {
                        Type = type,
                        Team = teamB,
                        CounterTeam = teamA,
                        Champ = champ,
                        Odd = float.Parse(odds[1].Text)
                    };

                    bets.Add(bet2);
                }

                // Mapa X Handicap
                else if (type.StartsWith($"{market} Handicap"))
                {

                    var handicaps = cp.FindElements(By.ClassName("handicap"));

                    var bet1 = new Bet
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = handicaps[1].Text,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet1);

                    var bet2 = new Bet
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = handicaps[2].Text,
                        Odd = float.Parse(odds[1].Text)
                    };

                    bets.Add(bet2);

                }

                // Mapa X Total de Rounds
                else if (type.StartsWith($"{market} Total de Rounds"))
                {

                    var handicaps = cp.FindElements(By.ClassName("handicap"));
                    var outcomeHeaders = cp.FindElements(By.ClassName("outcomeHeader"));

                    var bet1 = new Bet()
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = outcomeHeaders[0].Text + " " + handicaps[1].Text,
                        Odd = float.Parse(odds[0].Text)
                    };

                    bets.Add(bet1);

                    var bet2 = new Bet()
                    {
                        Type = type,
                        Team = teamA,
                        CounterTeam = teamB,
                        Champ = champ,
                        Detail = outcomeHeaders[1].Text + " " + handicaps[2].Text,
                        Odd = float.Parse(odds[1].Text)
                    };

                    bets.Add(bet2);

                }

            }

            return bets;
        }

        public static List<Bet> GetOtherMarketBets(IWebDriver driver, string champ, string teamA, string teamB) 
        {
            return new List<Bet>();
        }

        public static string GetChamp(IWebDriver driver) 
        {
            // Get Champ
            var itemTitle = driver.WaitToFindElements(
                ts: TimeSpan.FromSeconds(5),
                by: By.ClassName("itemTitle"),
                min: 3,
                ignoreException: false);

            return itemTitle[3].Text;
        }

        public static Tuple<string, string> GetTeams(IWebDriver driver) 
        {
            var outcomeHeaders = driver.WaitToFindElements(
                ts: TimeSpan.FromSeconds(5),
                by: By.ClassName("outcomeHeader"),
                min: 2,
                ignoreException: false);

            var teamA = outcomeHeaders[0].Text;
            var teamB = outcomeHeaders[1].Text;

            return new Tuple<string, string>(teamA, teamB);
        }
    }
}
