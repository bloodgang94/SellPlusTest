using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;
using SellPlusTest.Selenium.WebDriver.DriverOption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.WebDriver.WebDriverFactory
{
    public class RemoteWebDriverFactory : IRemoteWebDriverFactory
    {
        public Uri SelenoidUri { get; set; }
        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        public RemoteWebDriverFactory(Uri selenoidUri)
        {
            DriverOptionsFactory = new DriverOptionsFactory();
            SelenoidUri = selenoidUri;
        }

        public IWebDriver GetWebDriver(Browser browser)
        {
            return browser.BrowserType switch
            {
                BrowserType.Firefox => new RemoteWebDriver(SelenoidUri, DriverOptionsFactory.GetDriverOptions<FirefoxOptions>(browser)),
                BrowserType.Opera => new RemoteWebDriver(SelenoidUri, DriverOptionsFactory.GetDriverOptions<OperaOptions>(browser)),
                BrowserType.Chrome => new RemoteWebDriver(SelenoidUri, DriverOptionsFactory.GetDriverOptions<ChromeOptions>(browser)),
                _ => throw new PlatformNotSupportedException($"{browser} is not currently supported."),
            };
        }
        
    }
}
