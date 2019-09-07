using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.Generic;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Linq;

namespace SeleniumHomeTask
{
    [TestFixture]
    public class TaskFourteenTest
    {
        private IWebDriver driver;

        public void GotoNewPageAndReturn(IWebDriver driver, IWebElement link, WebDriverWait wait)
        {
            string mainWindow = driver.CurrentWindowHandle;
            ICollection<string> oldWindows = driver.WindowHandles;
            link.Click();
            wait.Until(c => c.WindowHandles.Count > oldWindows.Count); //Ожидание того, что количсетво страниц больше, чем предыдущее
            driver.SwitchTo().Window(driver.WindowHandles.Last()); 
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1"))); //Ожидание того, что новая страница прогрузилась
            driver.Close();
            driver.SwitchTo().Window(mainWindow);
        }
    

        [SetUp]
        public void Start()
        {
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [Test]
        public void Windows()
        {
            //Стартовая страница
            driver.Url = "http://localhost/litecart/admin/";
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            if (driver.FindElements(By.Name("login")).Count != 0)
            { //Проверка для Edge, чтобы убедиться, что пользователь не был залогинен
                //Авторизация в системе
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("remember_me")).Click();
                driver.FindElement(By.Name("login")).Click();
            }
            
            driver.FindElement(By.CssSelector("a[href$='app=countries&doc=countries']")).Click(); //Переход на страницу со странами
            driver.FindElement(By.CssSelector("a[href$='doc=edit_country']")).Click(); //Создание новой страны
            var externalLinks = driver.FindElements(By.XPath("//i[contains(@class, 'fa-external-link')]/parent::*")); //Список всех ссылок, которые ведут на другую страницу
            int linkQuan = externalLinks.Count;
            for (int i = 0; i < linkQuan; i++)
            {
                externalLinks = driver.FindElements(By.XPath("//i[contains(@class, 'fa-external-link')]/parent::*"));
                GotoNewPageAndReturn(driver, externalLinks[i], wait);
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
