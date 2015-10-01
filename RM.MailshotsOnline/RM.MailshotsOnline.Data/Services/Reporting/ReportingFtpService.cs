using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class ReportingFtpService : FtpService, IReportingFtpService
    {
        private static readonly string Key = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" +
                                             Constants.Constants.Reporting.PrivateKey;

        public ReportingFtpService(ILogger logger)
            : base(Key, Constants.Constants.Reporting.SftpUsername, Constants.Constants.Reporting.SftpHostname, logger)
        {
        }
    }
}
