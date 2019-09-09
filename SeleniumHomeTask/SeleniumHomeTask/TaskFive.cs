using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace SeleniumHomeTask
{
    [TestFixture]
    public class TaskFiveTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            // старая схема:
            FirefoxOptions options = new FirefoxOptions();
        //    options.UseLegacyImplementation = false;
            //options.BrowserExecutableLocation = @"‪C:\Program Files\Mozilla Firefox\firefox.exe";
            driver = new FirefoxDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Menu()
        {
            //Стартовая страница
            driver.Url = "http://localhost/litecart/admin/";
            if (driver.FindElements(By.Name("login")).Count != 0)
            { //Проверка для Edge, чтобы убедиться, что пользователь не был залогинен
                //Авторизация в системе
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("remember_me")).Click();
                driver.FindElement(By.Name("login")).Click();
            }
            var menu = driver.FindElements(By.CssSelector("#box-apps-menu > li > a")); //выделение блока с меню
            for (int i = 0; i < menu.Count; i++)
            {
                menu[i].Click();
                Assert.AreNotEqual(0, driver.FindElements(By.CssSelector("h1")), "Проблемы со страницей");
                var submenu = driver.FindElements(By.CssSelector("#box-apps-menu .docs a"));
                if (submenu.Count > 0)
                {
                    for (int j = 0; j < submenu.Count; j++)
                    {
                        submenu[j].Click();
                        Assert.AreNotEqual(0, driver.FindElements(By.CssSelector("h1")), "Проблемы со страницей");
                        submenu = driver.FindElements(By.CssSelector("#box-apps-menu .docs a"));
                    }
                }
                menu = driver.FindElements(By.CssSelector("#box-apps-menu > li > a"));
            }

            driver.FindElement(By.CssSelector("[title='Logout']")).Click();

        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
