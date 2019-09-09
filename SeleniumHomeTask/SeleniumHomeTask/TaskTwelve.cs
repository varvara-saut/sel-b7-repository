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
    public class TaskTwelveTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [Test]
        public void Countries()
        {      
            string startupPath4 = System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().Location);
            string startupPath5 = AppDomain.CurrentDomain.BaseDirectory;
        
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
            //Переход на страницу со странами
            driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=catalog']")).Click();
            driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=edit_product']")).Click();





            driver.FindElement(By.CssSelector("input[name='name[en]']")).SendKeys("tt123");
            driver.FindElement(By.CssSelector("input[type='file']")).SendKeys("C:\\Selenium\\sel-b7-repository\\SeleniumHomeTask\\SeleniumHomeTask\\duck.jpg");

            driver.FindElement(By.CssSelector("button[name='save']")).Click();
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
