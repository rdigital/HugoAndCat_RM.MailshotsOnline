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
        private static readonly string Key =
            $"{System.Web.Hosting.HostingEnvironment.MapPath("~/bin")}\\Keys\\{Constants.Constants.Reporting.PrivateKeyFileName}";

        public ReportingFtpService(ILogger logger)
            : base(Key, Constants.Constants.Reporting.SftpUsername, Constants.Constants.Reporting.SftpHostname, logger)
        {
        }
    }
}
