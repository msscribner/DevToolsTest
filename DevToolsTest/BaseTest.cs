using DevToolsTest;
using log4net;
using System.Collections.Generic;
using System.Configuration;

namespace AutomationTests.Base
{


    public class BaseTest
    {
        private static readonly ILog _log = LogManager.GetLogger(nameof(BaseTest));

        public BaseTest()
        {
            // Establish the Log4Net folder
            var logPath = ConfigurationManager.AppSettings["LogPath"];                      // "C:\ProgramData\Selenium"
            var environmentName = ConfigurationManager.AppSettings["EnvironmentName"];      // "OctopusMaintenance"

            // Update the Log4Net configuration
            log4net.GlobalContext.Properties["LogPath"] = logPath;
            log4net.GlobalContext.Properties["EnvironmentName"] = environmentName;

            // Make the Log4Net configuration active
            log4net.Config.XmlConfigurator.Configure();
        }


        public static IEnumerable<DriverType> GetDriver()
        {
            var driverTypes = new[] { DriverType.Chrome };

            foreach (var driverType in driverTypes)
            {
                yield return driverType;
            }
        }



    }
}
