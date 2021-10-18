using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.PageObject.Home
{
    public class HomePage : Page
    {
        public HomePage(IWebDriver driver) : base(driver) { }
        
        public IWebElement MainText => _driver.FindElement(By.XPath("//div[@class='hint-text_main']"));

    }
}
