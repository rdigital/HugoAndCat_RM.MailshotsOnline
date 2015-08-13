using HC.RM.Common.PCL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Test.Mocks
{
    public class MockLogger : ILogger
    {
        public void Critical(string className, string methodName, string message, params object[] args)
        {
            // Do nothing
        }

        public void Error(string className, string methodName, string message, params object[] args)
        {
            // Do nothing
        }

        public void Exception(string className, string methodName, Exception exception)
        {
            // Do nothing
        }

        public void Info(string className, string methodName, string message, params object[] args)
        {
            // Do nothing
        }

        public void Warn(string className, string methodName, string message, params object[] args)
        {
            // Do nothing
        }
    }
}
