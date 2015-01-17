using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class KittenPurchase
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        
        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "http://berryintl.dinafberry.com/books/";
            verificationErrors = new StringBuilder();
        }
        
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [Test]
        public void TheKittenPurchaseTest()
        {
            driver.Navigate().GoToUrl("http://kitten.local/");
            Assert.AreEqual("Home", driver.Title);
            driver.FindElement(By.LinkText("All products")).Click();
            Assert.AreEqual("All products", driver.Title);
            driver.FindElement(By.XPath("//section[@id='content']/ul/li[2]/a/div/div/h2")).Click();
            Assert.AreEqual("Tabbytacular", driver.Title);
            new SelectElement(driver.FindElement(By.Id("0daaf6c5-7490-4917-85c7-3997f9d21274"))).SelectByText("orange");
            new SelectElement(driver.FindElement(By.Id("d8eaf41d-ba40-4a66-bf43-d640adf6e4c8"))).SelectByText("extra long");
            driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
            Assert.AreEqual("Basket", driver.Title);
            driver.FindElement(By.LinkText("Checkout Now")).Click();
            Assert.AreEqual("Checkout", driver.Title);
            driver.FindElement(By.Name("contactFirst")).Clear();
            driver.FindElement(By.Name("contactFirst")).SendKeys("dina");
            driver.FindElement(By.Name("contactLast")).Clear();
            driver.FindElement(By.Name("contactLast")).SendKeys("berry");
            driver.FindElement(By.Name("contactEmail")).Clear();
            driver.FindElement(By.Name("contactEmail")).SendKeys("dinaberry@outlook.com");
            driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
            driver.FindElement(By.Name("shippingFirst")).Clear();
            driver.FindElement(By.Name("shippingFirst")).SendKeys("dina");
            driver.FindElement(By.Name("shippingLast")).Clear();
            driver.FindElement(By.Name("shippingLast")).SendKeys("berry");
            driver.FindElement(By.Name("shippingAddress1")).Clear();
            driver.FindElement(By.Name("shippingAddress1")).SendKeys("2511 main street");
            driver.FindElement(By.Name("shippingAddress2")).Clear();
            driver.FindElement(By.Name("shippingAddress2")).SendKeys("suite 1");
            driver.FindElement(By.Name("shippingCity")).Clear();
            driver.FindElement(By.Name("shippingCity")).SendKeys("bellingham");
            new SelectElement(driver.FindElement(By.Name("shippingRegion"))).SelectByText("Washington");
            driver.FindElement(By.Name("shippingPostalCode")).Clear();
            driver.FindElement(By.Name("shippingPostalCode")).SendKeys("98225");
            new SelectElement(driver.FindElement(By.Name("shippingCountry"))).SelectByText("United States");
            driver.FindElement(By.CssSelector("div.fields.open > input[type=\"submit\"]")).Click();
            new SelectElement(driver.FindElement(By.Name("shippingMethod"))).SelectByText("Flyweight 2nd Day Air ($10.00)");
            driver.FindElement(By.CssSelector("div.fields.open > input[type=\"submit\"]")).Click();
            new SelectElement(driver.FindElement(By.Name("payment"))).SelectByText("COD");
            driver.FindElement(By.CssSelector("div.fields.open > input[type=\"submit\"]")).Click();
            driver.FindElement(By.CssSelector("fieldset.summary > input[type=\"submit\"]")).Click();
            Assert.AreEqual("Receipt", driver.Title);
            try
            {
                Assert.AreEqual("Total", driver.FindElement(By.CssSelector("tr.total > td")).Text);
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            // ERROR: Caught exception [ERROR: Unsupported command [getTable | css=table.order-summary.4.1 | ]]
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
