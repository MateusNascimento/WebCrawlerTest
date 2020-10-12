using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebCrawlerTest.Entities;
using WebCrawlerTest.Scrappers;

namespace WebCrawlerTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            //// INITIAL VARIABLES
            var bets = new List<Bet>();
            //var mainUrl = "https://sports.betway.com/pt/sports/sct/esports/cs-go";

            //// GET CHAMP URLS (async)
            //var champUrls = await UrlScrapper.GetChampUrlsAsync(mainUrl);

            ////GET MATCH URLS (async)
            //var champUrlsTasks = new List<Task<List<string>>>();
            //var matchUrls = new List<string>();

            //foreach (var cu in champUrls)
            //{
            //    champUrlsTasks.Add(UrlScrapper.GetMatchUrlsAsync(cu));
            //}

            //while (champUrlsTasks.Any())
            //{
            //    var task = await Task.WhenAny(champUrlsTasks);
            //    champUrlsTasks.Remove(task);
            //    matchUrls.AddRange(await task);
            //}

            // SET SELENIUM
            var options = new EdgeOptions();
            options.UseChromium = true;
            //options.AddArgument("headless");
            var driver = new EdgeDriver(options);
            var actions = new Actions(driver);

            //var options = new ChromeOptions();
            //options.AddArgument("headless");
            //var driver = new ChromeDriver(options);
            //var actions = new Actions(driver);

            var sw = new Stopwatch();
            sw.Start();
            
            driver.Url = "https://sports.betway.com/pt/sports/evt/6533905";

            var champ = BetScrapper.GetChamp(driver);
            var teams = BetScrapper.GetTeams(driver);

            var contentSelectorItems = driver.WaitToFindElements(
                TimeSpan.FromSeconds(5), By.XPath("//div[@class='contentSelectorItem']"));

            var anyMarketFound = false;
            foreach (var csi in contentSelectorItems)
            {
                var market = csi.Text;

                switch (market)
                {
                    case "Principais Mercados":
                        anyMarketFound = true;
                        csi.Click();
                        driver.WaitUntilIsSelected(TimeSpan.FromSeconds(5), csi, false);
                        bets.AddRange(BetScrapper.GetMainMarketBets(driver, champ, teams.Item1, teams.Item2));
                        break;
                    case "Mapa 1":
                    case "Mapa 2":
                    case "Mapa 3":
                    case "Mapa 4":
                    case "Mapa 5":
                        anyMarketFound = true;
                        csi.Click();
                        driver.WaitUntilIsSelected(TimeSpan.FromSeconds(5), csi, false);
                        bets.AddRange(BetScrapper.GetMapMarketBets(driver, market, champ, teams.Item1, teams.Item2));
                        break;
                    case "Other":
                        anyMarketFound = true;
                        csi.Click();
                        driver.WaitUntilIsSelected(TimeSpan.FromSeconds(5), csi, false);
                        bets.AddRange(BetScrapper.GetOtherMarketBets(driver, champ, teams.Item1, teams.Item2));
                        break;
                }
            }

            sw.Stop();

            driver.Close();
        }
    }
}
