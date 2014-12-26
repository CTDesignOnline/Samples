
namespace Merchello.StarterSite.Test.Selenium
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support;
    using NUnit;
    using NUnit.Framework;

    /// <summary>
    /// Home page test for kitten site test
    /// Merchello features only. 
    /// </summary>
    [TestFixture]
    public class HomePageTests : SeleniumTest
    {
 
        public HomePageTests() : base("Merchello.Kitten") { }
 
 
        [Test]
        public void Did_quicklinks_display () {

            // xpath to quicklink div
            var xpathToQuickLinks = "//*[@id=\'quick-links\']";
            
            // Act
            this.FirefoxDriver.Navigate().GoToUrl(this.GetAbsoluteUrl("/"));
            
            // Assert
            Assert.IsTrue(this.FirefoxDriver.FindElement(By.XPath(xpathToQuickLinks)).Displayed);
        }
    }
}
