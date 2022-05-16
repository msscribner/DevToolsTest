using DevToolsTest;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Remote;
using System;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V96.DevToolsSessionDomains;


namespace Automation.Common
{

    public class Driver
    {
        public static RemoteWebDriver remoteWebDriver { get; set; }
        private static readonly ILog _log = LogManager.GetLogger(nameof(Driver));


        /// <summary>
        /// Intialize the Browser
        /// </summary>
        /// <param name="driverType"></param>
        /// <param name="url"></param>
        /// <param name="hub"></param>
        /// <exception cref="Exception"></exception>
        public void InitBrowser(DriverType driverType, string url, string hub)
        {
            DriverOptions options = null;

            _log.Info($"InitBrowser() - DriverType=[{driverType}] url=[{url}] hub=[{hub}]");

            switch (driverType)
            {
                case DriverType.Chrome:
                    var chromeOptions = new ChromeOptions();

                    chromeOptions.AcceptInsecureCertificates = true;
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");       //Works but writes to c:\tmp   prevents (Page Crash) exception
                                                                                //                  chromeOptions.AddArgument("--headless");
                                                                                //                  chromeOptions.AddArgument("--disable-setuid-sandbox");    // Prevents file restrictions: https://qxf2.com/blog/chrome-not-reachable-error/
                    chromeOptions.AddArgument("--ignore-ssl-errors=yes");       //Ignore 'Your connection is not Private' warning on the Browser
                    chromeOptions.AddArgument("--ignore-certificate-errors");   //Ignore 'Your connection is not Private' warning on the Browser
                    chromeOptions.AddArgument("--start-maximized");             //Start the browser as Maximized

                    options = chromeOptions;
                    break;
            }

            try
            {
                remoteWebDriver = new RemoteWebDriver(new Uri(hub), options.ToCapabilities(), TimeSpan.FromMinutes(5));

                if (remoteWebDriver != null)
                {
                    remoteWebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    StartDevToolsConsoleLogging();
                    remoteWebDriver.Url = url;
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Exception thrown in method Driver::InitBrowser() [{ex}]");

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Set up the DevTools for Console Logging
        /// </summary>
        public static void StartDevToolsConsoleLogging()
        {
            try
            {
                // Cast the RemoteWebDriver to a IDevTools
                IDevTools iDevTools = remoteWebDriver as IDevTools;

                // Derive the Session, SessionId and Domain
                DevToolsSession devToolSession = iDevTools.GetDevToolsSession();
                var sessionId = devToolSession.EndpointAddress.Split('/')[4];
                var domains = devToolSession.GetVersionSpecificDomains<DevToolsSessionDomains>();       //THIS LINE THROWS EXCEPTION

#if false
                // Associate the Console with the Event Handler
                domains.Console.Enable();
                domains.Console.ClearMessages();
                domains.Console.MessageAdded += DevToolsConsoleMessageAdded;
#endif

                _log.Info($"Registering Console Event Handler for {sessionId.ToString(),-30}");
            }
            catch (Exception exception)
            {
                _log.Error($" Message=[{exception.Message}]");
                _log.Error($" InnerException=[{exception.InnerException}]");
                _log.Error($" StackTrace=[{exception.StackTrace}]");
            }
        }/*StartDevToolsConsoleLogging*/

    }
}
