using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Helpers;
using HC.RM.Common.PCL.Persistence;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class ReportingBlobService : BlobService, IReportingBlobService
    {
        public ReportingBlobService(IBlobStorage blobStorage)
            : base(blobStorage, Constants.Constants.Reporting.BlobContainer)
        {
        }
    }
}
