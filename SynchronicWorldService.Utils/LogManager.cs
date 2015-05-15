using System.IO;
using log4net;
using log4net.Config;

namespace SynchronicWorldService.Utils
{
    /// <summary>
    /// Log manager to log message, exception to a file stored according to the log4net.config file 
    /// inside the SynhronicWorldService project (default : C:\SynchronicWorld_LOGS\)
    /// </summary>
    public static class LogManager
    {
        #region properties
        /// <summary>
        /// Logger
        /// </summary>
        private static ILog _logger;

        /// <summary>
        /// Logger (public)
        /// </summary>
        public static ILog Logger
        {
            get
            {
                if (_logger == null)
                    ConfigureLogger();
                return _logger;
            }
        }
        #endregion

        #region configs
        /// <summary>
        /// Config for Synchronic World Service logger
        /// </summary>
        private static void ConfigureLogger()
        {
            var configFile = Directory.GetCurrentDirectory() + @"\log4net.config";

            XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(configFile));
            _logger = log4net.LogManager.GetLogger("SWLogger");
        }
        #endregion
    }
}
