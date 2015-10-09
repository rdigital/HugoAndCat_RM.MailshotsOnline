using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common.Azure.Persistence;
using Microsoft.Azure;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class ReportingBlobStorage : BlobStorage, IReportingBlobStorage
    {
        public ReportingBlobStorage()
            : base(CloudConfigurationManager.GetSetting("StorageConnectionString"))
        {
        }
    }
}
