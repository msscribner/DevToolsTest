using AutomationTests.Base;
using NUnit.Framework;
using Automation.Common;
using log4net;

namespace DevToolsTest
{
    public class GenericTests : BaseTest
    {
        private static readonly ILog _log = LogManager.GetLogger(nameof(GenericTests));


        [SetUp]
        public void Setup()
        {
            _log.Info($"Setup() Starting a new Test...");
        }


        [TearDown]
        public static void Cleanup()
        {
            _log.Info($"Cleanup() Entering....TearDown-Starting....url=[{Driver.remoteWebDriver.Url}]. SessionId=[{Driver.remoteWebDriver.SessionId}] ");

            if (Driver.remoteWebDriver != null)
                Driver.remoteWebDriver.Quit();

            _log.Info($"Cleanup() Exiting....TearDown-Complete");
        }


        [Test, TestCaseSource(typeof(BaseTest), nameof(GetDriver)), Retry(3)]
        public void GenericSampleTest_12345(DriverType driverType)
        {
            // Establish the URL
            var url = "https://EntMainApp.ps.lcl/InformRMS";

            // Establish the Selenium HUB
//          var hub = "http://localhost:4444/wd/hub";
            var hub = "http://172.16.50.123:4444/wd/hub";           //qadockermrg1
//          var hub = "http://172.16.50.67:4444/wd/hub";            //qadockermrga


            // Instantiate a new Driver
            var driver = new Driver();

            // Initialize the web driver and set the user
            driver.InitBrowser(driverType, url, hub);
        }






    }
}
