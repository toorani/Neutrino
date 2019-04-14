using System;
using Neutrino.Core;
using NLog;

namespace Neutrino.External.Sevices
{
    public static class ExtensionMethods
    {
        public static void Info(this ILogger logger
            , ExternalServices serviceName
            , string message
            , params object[] argument)
        {
            LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Info(message, argument);
        }

        public static void Warn(this ILogger logger, ExternalServices serviceName, string message, params object[] argument)
        {
            LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Warn(message, argument);
        }

        public static void Error(this ILogger logger, ExternalServices serviceName, Exception exception, string message = "", params object[] argument)
        {
            LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Error(exception, message, argument);
        }
        public static void Error(this ILogger logger, ExternalServices serviceName, string message = "", params object[] argument)
        {
            LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Error(message, argument);
        }
        public static void Trace(this ILogger logger, ExternalServices serviceName, string message = "", params object[] argument)
        {
            LogManager.Configuration.Variables["serviceName"] = serviceName.ToString();
            logger.Trace(message, argument);
        }
    }
}
