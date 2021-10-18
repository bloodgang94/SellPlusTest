using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;

namespace SellPlusTest.Selenium
{
    public class StableWebElement : IWebElement, ILocatable, ITakesScreenshot, IWrapsDriver, IWrapsElement
    {
        private readonly ISearchContext _searchContext;
        private readonly IWebElement _element;
        private readonly By _by;
        private readonly By _byParent;

        public StableWebElement(ISearchContext searchContext, IWebElement element, By by, By byParent)
        {
            _searchContext = searchContext;
            _element = element;
            _by = by;
            _byParent = byParent;
        }

        public string TagName => StaleOff(() => _element.TagName);

        public string Text
        {
            get
            {
                try
                {
                    return StaleOff(() => _element.Text);
                }

                catch (StaleElementReferenceException)
                {
                    return StaleOff(() => _element.Text);
                }

            }
        }

        public bool Enabled => StaleOff(() => _element.Enabled);
        public bool Selected => StaleOff(() => _element.Selected);
        public Point Location => StaleOff(() => _element.Location);
        public Size Size => StaleOff(() => _element.Size);
        public bool Displayed
        {
            get
            {
                try
                {
                    return StaleOff(() => _element.Displayed);
                }

                catch (StaleElementReferenceException)
                {
                    return StaleOff(() => _element.Displayed);
                }

            }
        }
        public Point LocationOnScreenOnceScrolledIntoView => ((ILocatable)_element).LocationOnScreenOnceScrolledIntoView;
        public ICoordinates Coordinates => ((ILocatable)_element).Coordinates;
        public IWebDriver WrappedDriver => ((IWrapsDriver)_element).WrappedDriver;
        public IWebElement WrappedElement => _element;
        public void Clear() => StaleOff(() => _element.Clear());
        public void Click()
        {
            try
            {
                StaleOff(() => _element.Click());
            }

            catch(StaleElementReferenceException)
            {
                StaleOff(() =>_searchContext.FindElement(_byParent).FindElement(_by).Click());
            }
        }

        public IWebElement FindElement(By by) => new StableWebElement(_searchContext, _element.FindElement(by), by, _by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => StaleOff(() => _element.FindElements(by));
        public string GetAttribute(string attributeName) => _element.GetAttribute(attributeName);
        public string GetCssValue(string propertyName) => _element.GetCssValue(propertyName);
        public string GetProperty(string propertyName) => _element.GetProperty(propertyName);
        public Screenshot GetScreenshot() => ((ITakesScreenshot)_searchContext).GetScreenshot();
        public void SendKeys(string text) 
        {
            try
            {
                StaleOff(() => _element.SendKeys(text));
            }

            catch (StaleElementReferenceException)
            {
                StaleOff(() => _searchContext.FindElement(_byParent).FindElement(_by).SendKeys(text));
               
            }
        }
        public void Submit() => _element.Submit();

        private static T StaleOff<T>(Func<T> func)
        {
            T result = default!;

            StaleOff(() =>
            {
                result = func.Invoke();

            });

            if (result is null)
                throw new WebDriverTimeoutException();

            return result;
        }

        public static void StaleOff(Action action, int timeout = 5)
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
                    continue;
                }

            }

        }


    }
}
