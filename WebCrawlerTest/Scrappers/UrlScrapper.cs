using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawlerTest.Scrappers
{
    public class UrlScrapper
    {
        public static List<string> GetChampUrls(string url)
        {
            // Access URL
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // Get ahref elements
            var aNodes = doc.DocumentNode.SelectNodes(
                "//a[contains(@href, 'https://sports.betway.com/pt/sports/grp/esports/cs-go/')]");

            // Filtering if it has 'href' value
            var urlChamps = new List<string>();
            foreach (var aNode in aNodes)
            {
                var urlChamp = aNode.Attributes["href"]?.Value;
                if (!string.IsNullOrEmpty(urlChamp))
                {
                    urlChamps.Add(urlChamp);
                }
            }

            // Dispose doc
            doc = null;
            GC.Collect();

            return urlChamps;
        }

        public static List<string> GetMatchUrls(string url)
        {
            // Access URL
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // Get ahref elements
            var aNodes = doc.DocumentNode.SelectNodes(
             "//a[contains(@href, 'https://sports.betway.com/pt/sports/evt/')]");

            // Filtering if it has 'href' value
            var urlMatches = new List<string>();
            foreach (var aNode in aNodes)
            {
                var urlMatch = aNode.Attributes["href"]?.Value;
                if (!string.IsNullOrEmpty(urlMatch))
                {
                    urlMatches.Add(urlMatch);
                }
            }

            // Dispose docChamp
            doc = null;
            GC.Collect();

            return urlMatches;
        }

    }
}
