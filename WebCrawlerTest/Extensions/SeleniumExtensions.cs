﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WaitHelpers = SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace WebCrawlerTest
{
    public static class SeleniumExtensions
    {
        public static ReadOnlyCollection<IWebElement> WaitToFindElements(this IWebDriver driver, TimeSpan ts, By by, int min = 1, bool ignoreException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, ts);

                //  wait until elements > 0
                var result = wait.Until(x =>
                {
                    try
                    { 
                        var elements = x.FindElements(by);

                        if (elements.Count >= min)
                        {
                            return elements;
                        }

                        return null;
                    }
                    catch
                    {
                        return null;
                    }
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

        public static IWebElement WaitToFindElement(this IWebDriver driver, TimeSpan ts, By by, bool ignoreException = true)
        {
            var wait = new WebDriverWait(driver, ts);

            try
            {
                var result = wait.Until(x => 
                { 
                    try
                    {
                        return x.FindElement(by);
                    }
                    catch
                    {
                        return null;
                    }
                });                

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

        public static void WaitUntilIsSelected(this IWebDriver driver, TimeSpan ts, IWebElement element, bool ignoreException = true)
        {

            try
            {
                var wait = new WebDriverWait(driver, ts);
                wait.Until((d) =>
                {
                    try
                    {
                        var a = element.GetAttribute("outerHTML");
                        if (element.GetAttribute("outerHTML").Contains("selected=\"true\""))
                        {
                            return element;
                        }

                        return null;
                    }
                    catch
                    {
                        return null;
                    }

                });
            }
            catch (WebDriverTimeoutException e)
            {
                if (!ignoreException)
                {
                    throw;
                }
            }
        }

        public static void WaitUntilMarketIsAvailable(this IWebDriver driver, TimeSpan ts, string keyword, bool ignoreException = true)
        {

            try
            {
                var wait = new WebDriverWait(driver, ts);
                wait.Until((d) =>
                {
                    try
                    {
                        var cp = driver.FindElement(By.ClassName("collapsablePanel"));

                        var title = cp.FindElement(By.ClassName("title")).Text;

                        if (title.Contains(keyword))
                        {
                            return cp;
                        }

                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch (WebDriverTimeoutException e)
            {
                if (!ignoreException)
                {
                    throw;
                }
            }
        }

    }
}
