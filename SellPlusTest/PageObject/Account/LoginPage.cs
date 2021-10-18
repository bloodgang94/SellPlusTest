using OpenQA.Selenium;
using SellPlusTest.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.PageObject.Account
{
    public class LoginPage : Page
    {
        public LoginPage(IWebDriver driver)
            : base(driver) {}

        public IWebElement Login => _driver.FindElement(By.Id("Username"));
        public IWebElement Password => _driver.FindElement(By.Id("Password"));
        public IWebElement Enter => _driver.FindElement(By.XPath("//button[@value ='login']"));
        public IWebElement RememberMe => _driver.FindElement(By.Id("//div[@class ='slider round']"));

        public void LoginAs(RoleType roleType)
        {
            var user = AppSettings.Instance.SellPlusSettings.Credentials.Find(x => x.Role == roleType);
            LoginAs(user.Login, user.Password);
        }
        public void LoginAs(string login, string password, bool isRemember = false)
        {
            Login.SendKeys(login);
            Password.SendKeys(password);

            if (isRemember)
                RememberMe.Click();

            Enter.Click();
        }
    }
}
