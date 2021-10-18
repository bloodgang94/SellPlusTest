using Allure.Commons;
using BoDi;
using OpenQA.Selenium;
using SellPlusTest.Selenium.WebDriver;
using SellPlusTest.Selenium.WebDriver.WebDriverFactory;
using SellPlusTest.Settings;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace SellPlusTest.Steps
{
    [Binding]
    public class Hooks
    {
        private IWebDriver _driver;
        private readonly AllureLifecycle _allureLifecycle;
        private readonly ScenarioContext _scenarioContext;
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _allureLifecycle = AllureLifecycle.Instance;
        }

        [BeforeScenario]
        public void Setup()
        {
            var param = Environment.GetEnvironmentVariable("Test_Browser");
            var browserType = (BrowserType)Enum.Parse(typeof(BrowserType), param);

            _driver = new WebDriverProxy(new RemoteWebDriverFactory(new Uri (AppSettings.Instance.SellPlusSettings.ConnectionStrings.SelenoidUrl))
                .GetWebDriver(new Browser() { BrowserType = browserType }), 10);
            _objectContainer.RegisterInstanceAs(_driver);
        }

        [AfterStep]
        public void AfterStep()
        {
            _allureLifecycle.AddAttachment(((WebDriverProxy)_driver).GetScreenshot());
        }

        [AfterScenario]
        public void TearDown()
        {
            _scenarioContext.TryGetValue(out TestResult testresult);

            _allureLifecycle.UpdateTestCase(testresult.uuid, tc =>
            {
                tc.name = _scenarioContext.ScenarioInfo.Title;
                tc.historyId = Guid.NewGuid().ToString();
            });

            _driver.Quit();
        }
    }
}
