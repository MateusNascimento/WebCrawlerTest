using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;

namespace WebCrawlerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var bets = new List<Bet>();

            // Set Selenium
            var options = new EdgeOptions();
            options.UseChromium = true;
            options.AddArgument("headless");
            IWebDriver driver = new EdgeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.Url = "https://sports.betway.com/pt/sports/evt/6396570";

            var itemTitle = driver.FindElements(By.ClassName("itemTitle"));
            var champ = itemTitle[3].Text;

            var teamA = string.Empty;
            var teamB = string.Empty;
            
            var collapsablePanels = driver.FindElements(By.ClassName("collapsablePanel"));

            foreach (var cp in collapsablePanels) 
            {
                var type = cp.FindElement(By.ClassName("title")).Text;
                var odds = cp.FindElements(By.ClassName("odds"));

                // Vencedor da Partida
                if (type.StartsWith("Vencedor da Partida"))
                {
                    var outcomeHeaders = cp.FindElements(By.ClassName("outcomeHeader"));
                    teamA = outcomeHeaders[0].Text;
                    teamB = outcomeHeaders[1].Text;                                      

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

            var url = "https://sports.betway.com/pt/sports/sct/esports/cs-go";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var aNodes = doc.DocumentNode.SelectNodes(
                "//a[contains(@href, 'https://sports.betway.com/pt/sports/grp/esports/cs-go/')]");

            var urlChamps = (from aNode in aNodes
                            where !string.IsNullOrEmpty(aNode.Attributes["href"]?.Value)
                            select aNode.Attributes["href"].Value).ToList();
            
            // Dispose doc
            doc = null;
            GC.Collect();

            foreach (var urlChamp in urlChamps) 
            {
                var webChamp = new HtmlWeb();
                var docChamp = webChamp.Load(urlChamp);

                var aTeamsNodes = docChamp.DocumentNode.SelectNodes(
                 "//a[contains(@href, 'https://sports.betway.com/pt/sports/evt/')]");


                var urlMatchs = (from aTeamNode in aTeamsNodes
                                 where !string.IsNullOrEmpty(aTeamNode.Attributes["href"]?.Value)
                                 select aTeamNode.Attributes["href"].Value).ToList();

                // Dispose docChamp
                docChamp = null;
                GC.Collect();

                foreach (var urlMatch in urlMatchs)
                {

                    Console.WriteLine($"Loading bet from url '{urlMatch}' ...");

                    driver.Url = urlMatch;
                    
                    //var teamA = driver.FindElement(By.XPath("/html/body/div/div/div[3]/div/div[1]/div/div[2]/div[4]/div/div[3]/div/div[2]/div/div[5]/div/div[1]/div[2]/div/div[3]/div[1]"));
                    //var teamB = driver.FindElement(By.XPath("/html/body/div/div/div[3]/div/div[1]/div/div[2]/div[4]/div/div[3]/div/div[2]/div/div[5]/div/div[1]/div[2]/div/div[3]/div[2]"));

                    var oddA = driver.FindElement(By.XPath("/html/body/div/div/div[3]/div/div[1]/div/div[2]/div[4]/div/div[3]/div/div[2]/div/div[5]/div/div[1]/div[2]/div/div[4]/div/div[1]/div[1]/div[2]/div/div[3]/div"));
                    var oddB = driver.FindElement(By.XPath("/html/body/div/div/div[3]/div/div[1]/div/div[2]/div[4]/div/div[3]/div/div[2]/div/div[5]/div/div[1]/div[2]/div/div[4]/div/div[1]/div[2]/div[2]/div/div[3]/div"));


                    var bet = new Bet
                    {
                        //TeamA = teamA.Text,
                        //TeamB = teamB.Text,
                        //OddA = float.Parse(oddA.Text),
                        //OddB = float.Parse(oddB.Text)

                    };

                    bets.Add(bet);

                    Console.WriteLine($"Bet loaded! {bet}");


                }

            }

            Console.WriteLine("Hello World!");
        }
    }

    public class Bet
    {
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Champ { get; set; }
        public string Team { get; set; }
        public string CounterTeam { get; set; }
        public float Odd { get; set; }
        public override string ToString()
        {
            return $"[Type: {Type}, Detail: {Detail}, " +
                $"Team: {Team}, CounterTeam: {CounterTeam}, Odd: {Odd}]";
        }

    }
}
