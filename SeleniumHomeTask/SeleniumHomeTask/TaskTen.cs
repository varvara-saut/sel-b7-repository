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
    public class TaskTenTest
    {
        private IWebDriver driver;

        public int[] ColorRGBToInt(String color)
        {
            String[] numbers = color.Replace("rgba(", "").Replace("rgb(", "").Replace(")", "").Split(',');
            int[] colorInt = new int[3];
            colorInt[0] = Convert.ToInt32(numbers[0]);
            colorInt[1] = Convert.ToInt32(numbers[1]);
            colorInt[2] = Convert.ToInt32(numbers[2]);
            return colorInt;
        }

        public bool IsGray(int[] color)
        {
            return (color[0] == color[1] && color[1] == color[2]);
        }

        public bool IsRed(int[] color)
        {
            return (color[0] > 0 && color[1] == 0 && color[2] == 0);
        }

        public double FontSize(String size)
        {
            return Convert.ToDouble(size.Remove(size.Length - 2));
        }


        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [Test]
        public void CheckNames()
        {
            //Стартовая страница
            driver.Url = "http://localhost/litecart/";

            //Выбор правильного товара
            var product = driver.FindElement(By.CssSelector("#box-campaigns .product"));
            //Получение информации о товаре с главной страницы
            string nameMainPage = product.FindElement(By.CssSelector(".name")).GetAttribute("textContent");

            //Переход на страницу товара
            product.FindElement(By.CssSelector(".link")).Click();
            //Получение информации о товаре со страницы товара
            string nameProdPage = driver.FindElement(By.CssSelector("#box-product .title")).GetAttribute("textContent");

            //Сравнение
            Assert.AreEqual(nameMainPage, nameProdPage, "Названия товаров не совпадают");
        }

        [Test]
        public void CheckPrices()
        {
            //Стартовая страница
            driver.Url = "http://localhost/litecart/";

            //Выбор правильного товара
            var product = driver.FindElement(By.CssSelector("#box-campaigns .product"));
            //Получение информации о товаре с главной страницы
            var regPriceMainPage = product.FindElement(By.CssSelector(".regular-price"));
            string regularPriceMainPage = regPriceMainPage.GetAttribute("textContent");
            var camPriceMainPage = product.FindElement(By.CssSelector(".campaign-price"));
            string campaignPriceMainPage = camPriceMainPage.GetAttribute("textContent");

            //Переход на страницу товара
            product.FindElement(By.CssSelector(".link")).Click();
            //Получение информации о товаре со страницы товара
            var regPriceProdPage = driver.FindElement(By.CssSelector(".regular-price"));
            string regularPriceProdPage = regPriceProdPage.GetAttribute("textContent");
            var camPriceProdPage = driver.FindElement(By.CssSelector(".campaign-price"));
            string campaignPriceProdPage = camPriceProdPage.GetAttribute("textContent");

            //Сравнение
            Assert.AreEqual(regularPriceMainPage, regularPriceProdPage, "Обычные товаров не совпадают");
            Assert.AreEqual(campaignPriceMainPage, campaignPriceProdPage, "Акционные цены товаров не совпадают");
        }

        [Test]
        public void CheckPricesFont()
        {

            //Стартовая страница
            driver.Url = "http://localhost/litecart/";

            //Выбор правильного товара
            var product = driver.FindElement(By.CssSelector("#box-campaigns .product"));
            //Получение информации о товаре с главной страницы
            var regPriceMainPage = product.FindElement(By.CssSelector(".regular-price"));
            var camPriceMainPage = product.FindElement(By.CssSelector(".campaign-price"));

            //Проверка цен на главной странице
            Assert.IsTrue(IsGray(ColorRGBToInt(regPriceMainPage.GetCssValue("color"))), "Обычная цена не серая");
            Assert.IsTrue(regPriceMainPage.GetCssValue("text-decoration").Contains("line-through"), "Обычная цена не зачеркнута");
            Assert.IsTrue(IsRed(ColorRGBToInt(camPriceMainPage.GetCssValue("color"))), "Акционная цена не красная");
            Assert.GreaterOrEqual(Convert.ToInt32(camPriceMainPage.GetCssValue("font-weight")), 700, "Акционная цена не выделена жирным шрифтом");
            Assert.Greater(FontSize(camPriceMainPage.GetCssValue("font-size")), FontSize(regPriceMainPage.GetCssValue("font-size")), "Размер шрифта акционной цены меньше");

            //Переход на страницу товара
            product.FindElement(By.CssSelector(".link")).Click();
            //Получение информации о товаре со страницы товара
            var regPriceProdPage = driver.FindElement(By.CssSelector(".regular-price"));
            var camPriceProdPage = driver.FindElement(By.CssSelector(".campaign-price"));

            //Проверка цен на странице товара
            Assert.IsTrue(IsGray(ColorRGBToInt(regPriceProdPage.GetCssValue("color"))), "Обычная цена не серая");
            Assert.IsTrue(regPriceProdPage.GetCssValue("text-decoration").Contains("line-through"), "Обычная цена не зачеркнута");
            Assert.IsTrue(IsRed(ColorRGBToInt(camPriceProdPage.GetCssValue("color"))), "Акционная цена не красная");
            Assert.GreaterOrEqual(Convert.ToInt32(camPriceProdPage.GetCssValue("font-weight")), 700, "Акционная цена не выделена жирным шрифтом");
            Assert.Greater(FontSize(camPriceProdPage.GetCssValue("font-size")), FontSize(regPriceProdPage.GetCssValue("font-size")), "Размер шрифта акционной цены меньше");
        }
           
        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
