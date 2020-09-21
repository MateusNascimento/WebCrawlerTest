using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WaitHelpers = SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace WebCrawlerTest
{
    public static class SeleniumExtensions
    {
        public static ReadOnlyCollection<IWebElement> WaitToFindElements(this IWebDriver driver, int seconds, By by, bool ignoreException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

                //  wait until elements > 0
                var result = wait.Until(x =>
                {
                    var elements = x.FindElements(by);

                    if (elements.Count > 0)
                    {
                        return elements;
                    }

                    return null;

                });

                return result;

            }
            catch (WebDriverTimeoutException e)
            {
                if (ignoreException)
                {
                    return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
                }
                else
                {
                    throw;
                }                
            }            
        }

        public static IWebElement WaitToFindElement(this IWebDriver driver, int seconds, By by, bool ignoreException = true)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            try
            {
                var result = wait.Until(x => x.FindElement(by));

                return result;

            }
            catch (WebDriverTimeoutException e)
            {
                if (ignoreException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

    }
}
