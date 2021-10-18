using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.WebDriver.DriverOption
{
    public interface IDriverOptionsFactory
    {
        T GetDriverOptions<T>(Browser browser) where T : DriverOptions;
    }
}
