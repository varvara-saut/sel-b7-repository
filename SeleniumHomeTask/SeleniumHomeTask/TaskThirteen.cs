using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SeleniumHomeTask
{

    [TestFixture]
    public class TaskThirteenTest
    {
        private IWebDriver driver;

        public void AddToCart(IWebDriver driver, WebDriverWait wait)
        {
            driver.FindElement(By.CssSelector(".products .link")).Click();
            var quantity = driver.FindElement(By.CssSelector("#cart .quantity"));
            int quan = Convert.ToInt32(quantity.Text);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            if (driver.FindElements(By.CssSelector(".options select")).Count > 0)
            {
                var option = new SelectElement(driver.FindElement(By.CssSelector(".options select")));
                option.SelectByText("Small");
            }
            driver.FindElement(By.CssSelector(".quantity button")).Click();
            wait.Until(ExpectedConditions.TextToBePresentInElement(quantity, (quan+1).ToString())); //Ожидание обновления количество товаров в корзине
            driver.FindElement(By.CssSelector("#logotype-wrapper a")).Click();
        }

        public void RemoveFromCart(IWebDriver driver, WebDriverWait wait)
        {
            var table = driver.FindElement(By.CssSelector(".rounded-corners"));
            driver.FindElement(By.CssSelector("button[value='Remove']")).Click();
            wait.Until(ExpectedConditions.StalenessOf(table)); //Ожидание обновления количество товаров в корзине
        }

        [SetUp]
        public void Start()
        {
           // driver = new ChromeDriver();
            driver = new FirefoxDriver();
            //driver = new EdgeDriver();                     
        }

        [Test]
        public void Cart()
        {
            //Стартовая страница
            driver.Url = "https://litecart.stqa.ru/";
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            //Добавление товаров в корзину
            for (int i=0; i<3; i++)
                AddToCart(driver, wait);

            //Переход в корзину
            driver.FindElement(By.CssSelector("#cart .link")).Click();

            //Удаление товаров
            while (driver.FindElements(By.CssSelector(".rounded-corners")).Count > 0)
                RemoveFromCart(driver, wait);                    
        }

       
        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
