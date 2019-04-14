using System;


namespace Neutrino.Core.LogManagement
{
    public static class ExtensionMethods
    {
        public static void exInfo(this Ninject.Extensions.Logging.ILogger logger, ExternalServices serviceName, string message, params object[] argument)
        {
            NLog.LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Info(message, argument);
        }

        public static void exWarn(this Ninject.Extensions.Logging.ILogger logger, ExternalServices serviceName, string message, params object[] argument)
        {
            NLog.LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Warn(message, argument);
        }

        public static void exError(this Ninject.Extensions.Logging.ILogger logger, ExternalServices serviceName, Exception exception, string message="", params object[] argument)
        {
            NLog.LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Error(exception,message, argument);
        }
        public static void exError(this Ninject.Extensions.Logging.ILogger logger, ExternalServices serviceName, string message = "", params object[] argument)
        {
            NLog.LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Error(message, argument);
        }
        
    }
}
