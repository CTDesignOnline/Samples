The Merchello Starter site uses Selenium webdriver.

In order for the WebDriver to find the correct IISExpress port, you may need to start the website, note the port number, then change the private variable in SeleniumBase.

Currently, the tests use the FireFox driver. The Chrome and IE drivers are commented out.
