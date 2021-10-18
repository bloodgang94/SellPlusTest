using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using SellPlusTest.Selenium.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SellPlusTest.Selenium.WebDriver
{
    public class WebDriverProxy : IWebDriver, ISearchContext, IHasInputDevices, IActionExecutor, IJavaScriptExecutor
    {
        private readonly IWebDriver _driver;
        private readonly int _defaultTimeoutInSeconds;

        public SessionId SessionID => ((RemoteWebDriver)_driver).SessionId;

        public WebDriverProxy(IWebDriver driver, int defaultTimeoutInSeconds)
        {
            _driver = driver;
            _defaultTimeoutInSeconds = defaultTimeoutInSeconds;
        }

        public void SetFileDetector()
        {
            ((IAllowsFileDetection)_driver).FileDetector = new LocalFileDetector();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public string Url { get => _driver.Url; set => _driver.Url = value; }

        public string Title => _driver.Title;

        public string PageSource => _driver.PageSource;

        public string CurrentWindowHandle => _driver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public IKeyboard Keyboard => ((IHasInputDevices)_driver).Keyboard;

        public IMouse Mouse => ((IHasInputDevices)_driver).Mouse;

        public bool IsActionExecutor => ((IActionExecutor)_driver).IsActionExecutor;

        public void Close() => _driver.Close();

        public void Dispose() 
        {
            _driver.Dispose();
            GC.SuppressFinalize(this);
        } 

        public IWebElement FindElement(By by)
        {
            return _driver.WaitUntil(d => d.FindElement(by).TagName.Length > 0 
            ? new StableWebElement(d, d.FindElement(by), by, by) 
            : null , _defaultTimeoutInSeconds);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _driver.FindElements(by);
        }

        public ReadOnlyCollection<IWebElement> FindElementsVisible(By by, int defaultTimeoutInSeconds = 10)
        {
            return _driver.WaitUntil(d => d.FindElements(by).Any() ? d.FindElements(by) : null, defaultTimeoutInSeconds);
        }

        public IWebElement FindElementVisible(By by, int timeoutInSeconds = 30)
        {
            return _driver.WaitUntil(d => d.FindElement(by).Displayed
            ? new StableWebElement(d, d.FindElement(by), by, by)
            : null, timeoutInSeconds);
        }

        public IWebElement FindElementClickable(By by, int timeoutInSeconds = 30)
        {
            return _driver.WaitUntil(d => d.FindElement(by).Enabled
            ? new StableWebElement(d, d.FindElement(by), by, by)
            : null, timeoutInSeconds);
        }

        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();

        public void Quit() => _driver.Quit();

        public ITargetLocator SwitchTo() => _driver.SwitchTo();

        public void PerformActions(IList<ActionSequence> actionSequenceList) 
        {
            try
            {
                
                ((IActionExecutor)_driver).PerformActions(actionSequenceList);
            }
            catch(StaleElementReferenceException)
            {
                ((IActionExecutor)_driver).PerformActions(actionSequenceList);
            }
        }

        public void ResetInputState() => ((IActionExecutor)_driver).ResetInputState();

        public object ExecuteScript(string script, params object[] args) => ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);

        public object ExecuteAsyncScript(string script, params object[] args) => ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);

        public ISearchContext WaitForLoading(int timeoutInSeconds = 30)
        {
            return _driver.WaitUntil(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete") ? d : null, timeoutInSeconds);
        }

        public IWebDriver GoToLastWindow()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            return _driver;
        }

        public void ScrollTo(int xPosition, int yPosition)
        {
            var js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript(String.Format("window.scrollTo({0}, {1})", xPosition, yPosition));
        }

        public void ScrollToHight()
        {
            var js = ((IJavaScriptExecutor)_driver);
            js.ExecuteScript("window.scrollTo(0, -document.body.scrollHeight);");
        }

        public void ScrollToDown()
        {
            var js = ((IJavaScriptExecutor)_driver);
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }
        public void HideElement(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript($"arguments[0].style.visibility='hidden'", element);
        }

        public string GetScreenshot()
        {
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Guid.NewGuid().ToString());
            string fileName = $"{fileNameWithoutExtension}.png";
            string tempPath = Directory.GetCurrentDirectory() + "//Screenshots//";
            Directory.CreateDirectory(tempPath);
            var screenPath = Path.Combine(tempPath, fileName);
            screenshot.SaveAsFile(Path.Combine(tempPath + fileName), ScreenshotImageFormat.Png);

            return screenPath;
        }


    }
}
