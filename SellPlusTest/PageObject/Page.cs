using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.PageObject
{
    public abstract class Page
    {
        protected readonly IWebDriver _driver;
        public string Title => _driver.Title;

        public Page(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
