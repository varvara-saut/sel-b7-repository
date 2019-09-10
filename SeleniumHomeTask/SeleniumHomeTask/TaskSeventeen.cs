using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;


namespace SeleniumHomeTask
{
    [TestFixture]
    public class TaskSeventeenTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver(); //Версия хрома 72.0.3626.121 
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Logs()
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

            //Переход в каталог
            driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=catalog']")).Click();
            driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=catalog&category_id=1']")).Click();

            var products = driver.FindElements(By.CssSelector("a[href*='category_id=1&product_id']:not([title='Edit'])"));
            var count = products.Count;

            //Перебор продуктов в категории
            for (int i = 0; i < count; i++)
            {
                products[i].Click();
                foreach (LogEntry l in driver.Manage().Logs.GetLog(LogType.Browser))
                {
                    Assert.IsEmpty(l.Message, l.Message); //проверка, что лог не пустой
                }
                driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=catalog']")).Click();
                driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=catalog&category_id=1']")).Click();
                products = driver.FindElements(By.CssSelector("a[href*='category_id=1&product_id']:not([title='Edit'])"));
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
