using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.WebDriver
{
    public enum BrowserType
    {
        Firefox,
        Chrome,
        Opera
    }

    public class Browser
    {
        public BrowserType BrowserType { get; set; }
        public string Version { get; set; }
    }
}
