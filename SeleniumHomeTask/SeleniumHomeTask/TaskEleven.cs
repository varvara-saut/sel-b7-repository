using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Remote;

namespace SeleniumHomeTask
{

    [TestFixture]
    public class TaskElevenTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new EdgeDriver();

            //Запуск в облаке
            /*DesiredCapabilities capability = new DesiredCapabilities();
            capability.SetCapability("os", "Windows");
            capability.SetCapability("os_version", "10");
            capability.SetCapability("browser", "Firefox");
            capability.SetCapability("browser_version", "68.0");
            capability.SetCapability("browserstack.local", "false");
            capability.SetCapability("browserstack.selenium_version", "3.10.0");
            capability.SetCapability("browserstack.user", "");
            capability.SetCapability("browserstack.key", "");

            driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), capability);*/
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

        }

        [Test]
        public void NewUser()
        {
            //Стартовая страница
            driver.Url = "https://litecart.stqa.ru/en/";
            driver.FindElement(By.CssSelector("#box-account-login a")).Click(); //переход на страницу создания нового аккаунта

            //Заполнение полей нового аккаунта
            var account = driver.FindElement(By.CssSelector("#create-account"));
            var rand = new Random();
            int postfix = rand.Next(10000, 99999);
            account.FindElement(By.CssSelector("[name='firstname']")).SendKeys("FirstName" + postfix.ToString());
            account.FindElement(By.CssSelector("[name='lastname']")).SendKeys("LastName" + postfix.ToString());
            account.FindElement(By.CssSelector("[name='address1']")).SendKeys("Address" + postfix.ToString());
            account.FindElement(By.CssSelector("[name='postcode']")).SendKeys(postfix.ToString());
            account.FindElement(By.CssSelector("[name='city']")).SendKeys("Carson City");

            //Выбор из выпадающих списков
            driver.FindElement(By.CssSelector(".select2-selection")).Click();
            driver.FindElement(By.CssSelector(".select2-search__field")).SendKeys("United States" + Keys.Enter);
            var selZone = new SelectElement(account.FindElement(By.CssSelector("select[name='zone_code']")));
            selZone.SelectByText("Nevada");

            account.FindElement(By.CssSelector("[name='email']")).SendKeys("Email" + postfix.ToString() + "@test" + postfix.ToString() + ".com");
            account.FindElement(By.CssSelector("[name='phone']")).SendKeys("+1" + postfix.ToString());
            account.FindElement(By.CssSelector("[name='newsletter']")).Click();
            account.FindElement(By.CssSelector("[name='password']")).SendKeys("Password" + postfix.ToString());
            account.FindElement(By.CssSelector("[name='confirmed_password']")).SendKeys("Password" + postfix.ToString());

            account.FindElement(By.CssSelector("[name='create_account']")).Click();

            driver.FindElement(By.CssSelector(".content a[href*='logout']")).Click();

            //Повторный вход в новый аккаунт
            driver.FindElement(By.CssSelector("[name='email']")).SendKeys("Email" + postfix.ToString() + "@test" + postfix.ToString() + ".com");
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys("Password" + postfix.ToString());
            driver.FindElement(By.CssSelector("[name='login']")).Click();

            driver.FindElement(By.CssSelector(".content a[href*='logout']")).Click();

        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
