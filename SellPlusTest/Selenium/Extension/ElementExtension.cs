using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.Extension
{
    public static class ElementExtension
    {

        public static T WaitUntil<T>(this ISearchContext driver, Func<ISearchContext, T> condition, int timeout, string message = null)
        {
            var wait = new DefaultWait<ISearchContext>(driver)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                Message = message
            };

            wait.IgnoreExceptionTypes(new Type[]
            {
                typeof(NoSuchElementException),
                typeof(WebDriverTimeoutException),
                typeof(StaleElementReferenceException),
                typeof(ElementClickInterceptedException)
            });

            return wait.Until(condition);

        }

        public static void MoveToElementClick(this IWebElement element, IWebDriver driver)
        {
            var action = new Actions(driver);
            action.MoveToElement(element).Perform();
            action.Click(element).Perform();
        }

        public static IWebElement ScrollToView(this IWebElement element, IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return element;
        }

        public static IWebElement CheckVisibleValidateText(this IWebElement element, IWebDriver driver)
        {
            element.ScrollToView(driver).MoveToElementClick(driver);
            element.SendKeys("");

            var label = element.FindElement(By.XPath("./ancestor::div/label"));
            label.ScrollToView(driver).MoveToElementClick(driver); //Hack для вызова валидация у поля
            var searchElement = element.FindElement(By.XPath("./following::div[@class='el-form-item__error']"))
                   .GetAttribute("innerText");

            if (!(searchElement.Contains("Обязательно") || searchElement.Contains("Укажите")))
                throw new Exception($"Неверный текст валидации: {searchElement}");

            return element;
        }
        public static KeyValuePair<string, IWebElement>[] GetInput(IWebElement element, string xpath)
        {
            return element
                .FindElements(By.XPath(xpath))
                .Select(x => new KeyValuePair<string, IWebElement>(x.GetAttribute("id"), x))
                .ToArray();
        }

        public static IWebElement IsNotEmpty(this IWebElement element)
        {
            var textElement = element.GetAttribute("Value");
            if (string.IsNullOrEmpty(textElement))
                throw new Exception($"Элемент пустой");
            Console.WriteLine(textElement);
            return element;
        }
    }
}
