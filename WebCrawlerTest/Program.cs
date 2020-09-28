using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using WebCrawlerTest.Entities;
using WebCrawlerTest.Scrappers;

namespace WebCrawlerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // INITIAL VARIABLES
            var bets = new List<Bet>();
            //var mainUrl = "https://sports.betway.com/pt/sports/sct/esports/cs-go";

            // GET CHAMP URLS
            //var champUrls = GetChampUrls(mainUrl);

            //GET MATCH URLS
            //var matchUrls = champUrls.SelectMany(x => GetMatchUrls(x)).ToList();

            // SET SELENIUM
            var options = new EdgeOptions();
            options.UseChromium = true;
            //options.AddArgument("headless");
            var driver = new EdgeDriver(options);

            driver.Url = "https://sports.betway.com/pt/sports/evt/6441810";

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
                        driver.WaitUntilMarketIsAvailable(TimeSpan.FromSeconds(10), "Vencedor da Partida", false);
                        bets.AddRange(BetScrapper.GetMainMarketBets(driver));
                        break;
                    case "Mapa 1":
                        anyMarketFound = true;
                        csi.Click();
                        driver.WaitUntilMarketIsAvailable(TimeSpan.FromSeconds(10), market, false);
                        bets.AddRange(BetScrapper.GetMapMarketBets(driver, market));
                        break;
                    case "Mapa 2":
                        break;
                    case "Mapa 3":
                        break;
                    case "Mapa 4":
                        break;
                    case "Mapa 5":
                        break;
                    case "Other":
                        break;
                }
            }

        }

    }
}
