using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SecondClassFirstTask
{
    [TestFixture]
    public class MyFirstTest
    {
        private ChromeDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void FirstTest()
        {
            driver.Navigate().GoToUrl("https://trello.com");
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
