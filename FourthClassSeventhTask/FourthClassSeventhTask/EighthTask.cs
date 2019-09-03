using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace FourthClassEighthTask
{
    [TestFixture]
    public class EighthTaskTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [Test]
        public void StickerByProduct()
        {           
            driver.Url = "http://localhost/litecart/";
            var products = driver.FindElements(By.CssSelector(".product"));
            foreach (var product in products)
            {
                var stickers = product.FindElements(By.CssSelector(".sticker"));
                Assert.AreEqual(1, stickers.Count, "Houston, we have a problem with stickers");
            }
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
