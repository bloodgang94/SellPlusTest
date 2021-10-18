using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellPlusTest.Selenium.WebDriver.DriverOption
{
    public class DriverOptionsFactory : IDriverOptionsFactory
    {

        public T GetDriverOptions<T>(Browser browser) where T : DriverOptions => (T)SetOptions(browser);

        private static DriverOptions SetOptions(Browser browser)
        {
            DriverOptions options = null;
            return browser.BrowserType switch
            {
                BrowserType.Chrome => ((Func<DriverOptions>)(() => {
                    options = new ChromeOptions();
                    //((ChromeOptions)options).AddExtension(Environment.CurrentDirectory + "\\CryptoProExtension.crx");
                    ((ChromeOptions)options).AddAdditionalCapability("enableVNC", true, true);
                    ((ChromeOptions)options).AddArguments("--ignore-ssl-errors=yes", "--ignore-certificate-errors", "--start-maximized");
                    ((ChromeOptions)options).AddArgument("--profile-directory=Default");
                    ((ChromeOptions)options).AddArgument("--disable-notifications");
                    if (browser.Version is not null)
                        ((ChromeOptions)options).BrowserVersion = browser.Version;

                    return options;
                }))(),

                BrowserType.Firefox => ((Func<DriverOptions>)(() => {
                    options = new FirefoxOptions();
                    FirefoxProfile ffprofile = new FirefoxProfile()
                    {
                        AcceptUntrustedCertificates = true,
                        AssumeUntrustedCertificateIssuer = false,

                    };

                    ffprofile.SetPreference("browser.download.folderList", 2);
                    ffprofile.SetPreference("bbrowser.link.open_newwindow.override.external", 3);
                    ffprofile.SetPreference("bbrowser.link.open_newwindow", 3);
                    ffprofile.SetPreference("browser.download.panel.shown", false);
                    ffprofile.SetPreference("browser.download.dir", "/c:/home/selenium/Downloads");
                    ffprofile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf, " +
                        "application/zip, application/pdf, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/octet-stream");

                    ((FirefoxOptions)options).Profile = ffprofile;
                    ((FirefoxOptions)options).AddAdditionalCapability("enableVNC", true, true);
                    options.AcceptInsecureCertificates = true;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    if (browser.Version is not null)
                        ((FirefoxOptions)options).BrowserVersion = browser.Version;
                    return options;
                }))(),

                BrowserType.Opera => ((Func<DriverOptions>)(() => {
                    options = new OperaOptions();
                    ((OperaOptions)options).AddAdditionalCapability("enableVNC", true, true);
                    ((OperaOptions)options).AcceptInsecureCertificates = true;
                    if (browser.Version is not null)
                        ((OperaOptions)options).BrowserVersion = browser.Version;
                    return options;
                }))(),

                _ => throw new NotSupportedException()
            };

        }

    }
}
