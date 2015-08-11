using HC.RM.Common.Azure;
using HC.RM.Common.Azure.Extensions;
using HC.RM.Common.PCL.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Helpers
{
    [Obsolete("This class should be removed and ILogger should be used throughout the project once the Common libraries are updated with RoleEnvironment.IsAvailable")]
    public class TelemetryHelper
    {
        private ILogger _logger;

        /// <summary>
        /// Helper for Application Insights telemetry
        /// </summary>
        public TelemetryHelper()
        {
            if (Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.IsAvailable)
            {
                _logger = new Logger();
            }
        }

        /// <summary>
        /// Log an error
        /// </summary>
        /// <param name="className">Name of originating class</param>
        /// <param name="methodName">Method name</param>
        /// <param name="message">Message to log</param>
        /// <param name="args">Arguments</param>
        public void Error(string className, string methodName, string message, params object[] args)
        {
            if (_logger != null)
            {
                _logger.Error(className, methodName, message, args);
            }
        }

        /// <summary>
        /// Log info
        /// </summary>
        /// <param name="className">Name of originating class</param>
        /// <param name="methodName">Method name</param>
        /// <param name="message">Message to log</param>
        /// <param name="args">Arguments</param>
        public void Info(string className, string methodName, string message, params object[] args)
        {
            if (_logger != null)
            {
                _logger.Info(className, methodName, message, args);
            }
        }

        /// <summary>
        /// Log a warning
        /// </summary>
        /// <param name="className">Name of originating class</param>
        /// <param name="methodName">Method name</param>
        /// <param name="message">Message to log</param>
        /// <param name="args">Arguments</param>
        public void Warn(string className, string methodName, string message, params object[] args)
        {
            if (_logger != null)
            {
                _logger.Warn(className, methodName, message, args);
            }
        }

        /// <summary>
        /// Log a critical error
        /// </summary>
        /// <param name="className">Name of originating class</param>
        /// <param name="methodName">Method name</param>
        /// <param name="message">Message to log</param>
        /// <param name="args">Arguments</param>
        public void Critical(string className, string methodName, string message, params object[] args)
        {
            if (_logger != null)
            {
                _logger.Critical(className, methodName, message, args);
            }
        }

        /// <summary>
        /// Log an exception
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="exception"></param>
        public void Exception(string className, string methodName, Exception exception)
        {
            if (_logger != null)
            {
                _logger.Exception(className, methodName, exception);
            }
        }
    }
}
