using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;

namespace SellPlusTest.Selenium.Extension
{
    public static class WaitExtensions
    {
        
        public static void Wait(Action action, int timeout = 5)
        {
            var sw = Stopwatch.StartNew();

            while (sw.Elapsed < TimeSpan.FromSeconds(timeout))
            {
                try
                {
                    action?.Invoke();
                    return;
                }
                catch (Exception ex) when (ex is NoSuchElementException || ex is ElementClickInterceptedException || ex is ElementNotInteractableException)
                {
                    Console.WriteLine(ex);
                    continue;
                }

            }

        }

        public static T Wait<T>(Func<T> func)
        {
            T result = default!;
            Wait(() =>
            {
                result = func.Invoke();
            });

            if (result is null)
                throw new WebDriverTimeoutException();

            return result;
        }

    }
}
