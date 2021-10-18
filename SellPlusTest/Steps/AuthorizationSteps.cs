using FluentAssertions;
using OpenQA.Selenium;
using SellPlusTest.PageObject.Account;
using SellPlusTest.PageObject.Home;
using SellPlusTest.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SellPlusTest.Steps
{

    [Binding]
    public class AuthorizationSteps
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;

        public AuthorizationSteps(IWebDriver driver)
        {
            _driver = driver;
            _loginPage = new LoginPage(_driver);
        }

        [Given(@"I Navigate to the Login page")]
        public void GivenINavigateToTheLoginPage()
        {
            _driver.Navigate().GoToUrl(AppSettings.Instance.SellPlusSettings.ConnectionStrings.SiteUrl);
        }
        
        [When(@"I Log in under the Role:'(.*)'")]
        public void WhenILoginUsingUkGmail_ComAndPass(RoleType role)
        {
            _loginPage.LoginAs(role);
        }

        [Then(@"I should see title '(.*)' string")]
        public void ThenIShouldSeeNextAuthenticationSucces(string message)
        {
            var homePage = new HomePage(_driver);

            homePage.Title.Should().Contain(message);
        }

        [StepArgumentTransformation]
        public RoleType RoleTransform(string role)
        {
            if (!Enum.TryParse(role, out RoleType roleType))
                throw new ArgumentException("No Such role!");

            return roleType;
        }
    }
}
