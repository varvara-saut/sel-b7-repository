using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Windows;
using System.Threading;

namespace SeleniumHomeTask
{
    [TestFixture]
    public class TaskTwelveTest
    {
        private IWebDriver driver;

        public string DirectoryPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("\\bin\\Debug","");
            return path;
        }

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            
        }

        [Test]
        [STAThread]
        public void NewProduct()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            var rand = new Random();
            string postfix = rand.Next(10000, 99999).ToString();

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

            //Заполнение блока General
            driver.FindElement(By.CssSelector("[name='status'][value='1']")).Click(); //Status = Enabled
            driver.FindElement(By.Name("name[en]")).SendKeys("Test Duck " + postfix); //Name
            driver.FindElement(By.Name("code")).SendKeys(postfix); //Code
            driver.FindElement(By.CssSelector("[name='product_groups[]'][value='1-3']")).Click(); //Product Gropusp Gender = Unisex
            driver.FindElement(By.Name("quantity")).Clear();
            driver.FindElement(By.Name("quantity")).SendKeys(postfix); //Quantity
            driver.FindElement(By.CssSelector("input[type='file']")).SendKeys(DirectoryPath()+"duck.jpg");

            //Заполнение дат
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("arguments[0].value=arguments[1];", driver.FindElement(By.Name("date_valid_from")), DateTime.Today.ToString("yyyy-MM-dd"));
            executor.ExecuteScript("arguments[0].value=arguments[1];", driver.FindElement(By.Name("date_valid_to")), DateTime.Today.AddDays(365).ToString("yyyy-MM-dd"));


            //Заполнение блока Information
            driver.FindElement(By.CssSelector("a[href$='information']")).Click();
            wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(By.CssSelector(".tabs .active a")), "Information"));
            var manufacturer = new SelectElement(driver.FindElement(By.Name("manufacturer_id")));
            manufacturer.SelectByValue("1");
            driver.FindElement(By.Name("keywords")).SendKeys("Test duck");
            driver.FindElement(By.Name("short_description[en]")).SendKeys("Test duck");
            //Вставка большого объема текста из буфера обмена
            string descr = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sollicitudin ante massa, eget ornare libero porta congue. Cras scelerisque dui non consequat sollicitudin. Sed pretium tortor ac auctor molestie. Nulla facilisi. Maecenas pulvinar nibh vitae lectus vehicula semper. Donec et aliquet velit. Curabitur non ullamcorper mauris. In hac habitasse platea dictumst. Phasellus ut pretium justo, sit amet bibendum urna. Maecenas sit amet arcu pulvinar, facilisis quam at, viverra nisi. Morbi sit amet adipiscing ante. Integer imperdiet volutpat ante, sed venenatis urna volutpat a. Proin justo massa, convallis vitae consectetur sit amet, facilisis id libero.";
            Thread thread = new Thread(() => Clipboard.SetText(descr));
            thread.SetApartmentState(ApartmentState.STA); 
            thread.Start();
            thread.Join();
            driver.FindElement(By.Name("description[en]")).SendKeys(Keys.Control + "v");
            driver.FindElement(By.Name("head_title[en]")).SendKeys("Test duck");
            driver.FindElement(By.Name("meta_description[en]")).SendKeys("Test duck");


            //Заполнение блока Prices
            driver.FindElement(By.CssSelector("a[href$='prices']")).Click();
            wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(By.CssSelector(".tabs .active a")), "Prices"));
            driver.FindElement(By.Name("purchase_price")).Clear();
            driver.FindElement(By.Name("purchase_price")).SendKeys("100");
            var purPrice = new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code")));
            purPrice.SelectByText("US Dollars");
            driver.FindElement(By.Name("prices[USD]")).SendKeys("1000");

            //Сохранение
            driver.FindElement(By.CssSelector("button[name='save']")).Click();

            //Проверка в каталоге
            driver.FindElement(By.CssSelector("a[href$='app=catalog&doc=catalog']")).Click();
            string xpath = "//*[.='" + "Test Duck " + postfix + "']";
            Assert.GreaterOrEqual(driver.FindElements(By.XPath(xpath)).Count, 1,
                "We have a problem with new duck");

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
