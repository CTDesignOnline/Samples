
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
    using System.Diagnostics;
    using System.IO;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;

    /// <summary>
    /// http://stephenwalther.com/archive/2011/12/22/asp-net-mvc-selenium-iisexpress
    /// </summary>
    [TestFixture]
    public abstract class SeleniumTest
    {

        const int iisPort = 59414;
        private string _applicationName;
        private Process _iisProcess;

        public FirefoxDriver FirefoxDriver { get; set; }
        //public ChromeDriver ChromeDriver { get; set; }
        //public InternetExplorerDriver InternetExplorerDriver { get; set; }


        protected SeleniumTest(string applicationName)
        {
            _applicationName = applicationName;
        }

        [SetUp]
        public void TestInitialize()
        {
            // Start IISExpress
            StartIIS();

            // Start Selenium drivers
            this.FirefoxDriver = new FirefoxDriver();
            //this.ChromeDriver = new ChromeDriver();
            //this.InternetExplorerDriver = new InternetExplorerDriver();

        }


        [TearDown]
        public void TestCleanup()
        {
            // Ensure IISExpress is stopped
            if (_iisProcess.HasExited == false)
            {
                _iisProcess.Kill();
            }


            // Stop all Selenium drivers
            this.FirefoxDriver.Quit();
            //this.ChromeDriver.Quit();
            //this.InternetExplorerDriver.Quit();

        }



        private void StartIIS()
        {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + "\\IIS Express\\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:" + "{0}" + " /port:{1}", applicationPath, iisPort);
            _iisProcess.Start();
        }


        protected virtual string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }


        public string GetAbsoluteUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return String.Format("http://localhost:{0}{1}", iisPort, relativeUrl);
        }


    }
}
