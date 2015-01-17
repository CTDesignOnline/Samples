using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
[TestFixture]
public class kitten.purchase
{
private ISelenium selenium;
private StringBuilder verificationErrors;

[SetUp]
public void SetupTest()
{
selenium = new DefaultSelenium("localhost", 4444, "*chrome", "http://berryintl.dinafberry.com/books/");
selenium.Start();
verificationErrors = new StringBuilder();
}

[TearDown]
public void TeardownTest()
{
try
{
selenium.Stop();
}
catch (Exception)
{
// Ignore errors if unable to close the browser
}
Assert.AreEqual("", verificationErrors.ToString());
}

[Test]
public void TheKitten.purchaseTest()
{
			selenium.Open("http://kitten.local/");
			Assert.AreEqual("Home", selenium.GetTitle());
			selenium.Click("link=All products");
			selenium.WaitForPageToLoad("30000");
			Assert.AreEqual("All products", selenium.GetTitle());
			selenium.Click("//section[@id='content']/ul/li[2]/a/div/div/h2");
			selenium.WaitForPageToLoad("30000");
			Assert.AreEqual("Tabbytacular", selenium.GetTitle());
			selenium.Select("id=0daaf6c5-7490-4917-85c7-3997f9d21274", "label=orange");
			selenium.Select("id=d8eaf41d-ba40-4a66-bf43-d640adf6e4c8", "label=extra long");
			selenium.Click("css=input[type=\"submit\"]");
			selenium.WaitForPageToLoad("30000");
			Assert.AreEqual("Basket", selenium.GetTitle());
			selenium.Click("link=Checkout Now");
			selenium.WaitForPageToLoad("30000");
			Assert.AreEqual("Checkout", selenium.GetTitle());
			selenium.Type("name=contactFirst", "dina");
			selenium.Type("name=contactLast", "berry");
			selenium.Type("name=contactEmail", "dinaberry@outlook.com");
			selenium.Click("css=input[type=\"submit\"]");
			selenium.Type("name=shippingFirst", "dina");
			selenium.Type("name=shippingLast", "berry");
			selenium.Type("name=shippingAddress1", "2511 main street");
			selenium.Type("name=shippingAddress2", "suite 1");
			selenium.Type("name=shippingCity", "bellingham");
			selenium.Select("name=shippingRegion", "label=Washington");
			selenium.Type("name=shippingPostalCode", "98225");
			selenium.Select("name=shippingCountry", "label=United States");
			selenium.Click("css=div.fields.open > input[type=\"submit\"]");
			selenium.Select("name=shippingMethod", "label=Flyweight 2nd Day Air ($10.00)");
			selenium.Click("css=div.fields.open > input[type=\"submit\"]");
			selenium.Select("name=payment", "label=COD");
			selenium.Click("css=div.fields.open > input[type=\"submit\"]");
			selenium.Click("css=fieldset.summary > input[type=\"submit\"]");
			Assert.AreEqual("Receipt", selenium.GetTitle());
			try
			{
				Assert.AreEqual("Total", selenium.GetText("css=tr.total > td"));
			}
			catch (AssertionException e)
			{
				verificationErrors.Append(e.Message);
			}
			try
			{
				Assert.AreEqual("$25.00", selenium.GetTable("css=table.order-summary.4.1"));
			}
			catch (AssertionException e)
			{
				verificationErrors.Append(e.Message);
			}
}
}
}
