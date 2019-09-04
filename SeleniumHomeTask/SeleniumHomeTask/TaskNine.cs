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
    public class TaskNineTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);         
        }

        [Test]
        public void Countries()
        {
            //Стартовая страница
            driver.Url = "http://localhost/litecart/admin/";
            if (driver.FindElements(By.Name("login")).Count!=0) { //Проверка для Edge, чтобы убедиться, что пользователь не был залогинен
                //Авторизация в системе
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("remember_me")).Click();
                driver.FindElement(By.Name("login")).Click();
            }
            //wait.Until(c => c.FindElement(By.XPath("//title[text() ='My Store']")));
            //Переход на страницу со странами
            driver.FindElement(By.CssSelector("a[href$='app=countries&doc=countries']")).Click();
            //Работа с таблицей
            var tableCountries = driver.FindElement(By.CssSelector(".dataTable"));
            var rowsCountries = tableCountries.FindElements(By.CssSelector(".row"));
            var tempCountry = "";
            //Проверка алфавитного порядка стран
            for (int i = 0; i < rowsCountries.Count; i++)
            {
                var nameCountry = rowsCountries[i].FindElement(By.XPath("./td[5]")).Text;
                var zones = Convert.ToInt32(rowsCountries[i].FindElement(By.XPath("./td[6]")).Text);
                Assert.GreaterOrEqual(nameCountry, tempCountry, "We have a problem with countries names");
                tempCountry = nameCountry;
                //Обработка нескольких зон
                if (zones > 0)
                {
                    rowsCountries[i].FindElement(By.XPath("./td[5]/a")).Click(); //Переход на страницу со страной
                    var tableZones = driver.FindElement(By.CssSelector(".dataTable"));
                    var rowsZones = tableZones.FindElements(By.CssSelector("tr"));
                    var tempZone = "";
                    for (int j = 1; j < rowsZones.Count - 1; j++)
                    {
                        var nameZone = rowsZones[j].FindElement(By.XPath("./td[3]")).Text;
                        Assert.GreaterOrEqual(nameZone, tempZone, "We have a problem with zones names");
                        tempZone = nameZone;
                    }
                    driver.FindElement(By.CssSelector("a[href$='app=countries&doc=countries']")).Click();
                    tableCountries = driver.FindElement(By.CssSelector(".dataTable"));
                    rowsCountries = tableCountries.FindElements(By.CssSelector(".row"));
                }
            }

            driver.FindElement(By.CssSelector("[title='Logout']")).Click();

        }

        [Test]
        public void GeoZones()
        {
            //Стартовая страница
            driver.Url = "http://localhost/litecart/admin/";
            if (driver.FindElements(By.Name("login")).Count != 0)
            {
                //Авторизация в системе
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("remember_me")).Click();
                driver.FindElement(By.Name("login")).Click();
            }
            //Переход на страницу с геозонами
            driver.FindElement(By.CssSelector("a[href$='app=geo_zones&doc=geo_zones']")).Click();
            //Работа с таблицей
            var tableCountries = driver.FindElement(By.CssSelector(".dataTable"));
            var rowsCountries = tableCountries.FindElements(By.CssSelector(".row"));
            //Переход к геозонам по каждой стране из списка
            for (int i = 0; i < rowsCountries.Count; i++)
            {
                rowsCountries[i].FindElement(By.XPath("./td[3]/a")).Click(); //Переход на страницу со списком геозон страны

                var tableZones = driver.FindElement(By.CssSelector("#table-zones"));
                var rowsZones = tableZones.FindElements(By.CssSelector("tr"));
                var tempZone = "";
                for (int j = 1; j < rowsZones.Count - 1; j++)
                {
                    var nameZone = rowsZones[j].FindElement(By.CssSelector("select[name*='zone_code'] option[selected]")).Text;
                    Assert.GreaterOrEqual(nameZone, tempZone, "We have a problem with zones names");
                    tempZone = nameZone;
                }

                //Возврат к списку стран с геозонами
                driver.FindElement(By.CssSelector("a[href$='app=geo_zones&doc=geo_zones']")).Click();
                tableCountries = driver.FindElement(By.CssSelector(".dataTable"));
                rowsCountries = tableCountries.FindElements(By.CssSelector(".row"));
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
