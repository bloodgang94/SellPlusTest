using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.WebDriver.WebDriverFactory
{
    public interface IRemoteWebDriverFactory
    {
        Uri SelenoidUri{ get; set; }
        IWebDriver GetWebDriver(Browser browser);
    }
}
