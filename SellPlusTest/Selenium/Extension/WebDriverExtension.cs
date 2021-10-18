using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.Extension
{
    public static class WebDriverExtension
    {
        public static void Refresh(this IWebDriver driver, int count = 1)
        {
            int attempts = 0;
            while (attempts < count)
            {
                try
                {
                    driver.Navigate().Refresh();
                }
                catch { }

                finally
                {
                    attempts++;
                }
               
               
            }
        }
        public static IWebDriver GoToLastWindow(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            return driver;
        }

        public static void ScrollTo(IWebDriver driver, int xPosition, int yPosition)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(String.Format("window.scrollTo({0}, {1})", xPosition, yPosition));
        }

        public static void ScrollToHight(IWebDriver driver)
        {
            var js = ((IJavaScriptExecutor)driver);
            js.ExecuteScript("window.scrollTo(0, -document.body.scrollHeight);");
        }

        public static void ScrollToDown(IWebDriver driver)
        {
            var js = ((IJavaScriptExecutor)driver);
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }
        public static void HideElement(IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].style.visibility='hidden'", element);
        }

        public static string GetTittleTimeOut(this IWebDriver driver, string expected, int durationInSecond)
        {
            var startTime = DateTime.UtcNow;
            var breakDuration = TimeSpan.FromSeconds(durationInSecond);
            while (DateTime.UtcNow - startTime < breakDuration)
            {

                if (driver.Title.GetHashCode() == expected.GetHashCode())
                    return driver.Title;
            }
            return driver.Title;
        }

        public static IAlert AlertShown(this IWebDriver driver, int timeoutInSeconds = 10)
        {
            var wait = new DefaultWait<IWebDriver>(driver);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException), typeof(NoAlertPresentException));
            wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            return wait.Until(d => d.SwitchTo().Alert());
        }


    }
}
