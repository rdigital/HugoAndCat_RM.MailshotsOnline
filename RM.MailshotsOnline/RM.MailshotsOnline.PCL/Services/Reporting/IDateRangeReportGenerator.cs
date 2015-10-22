using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.MailshotsOnline.PCL.Models.Reporting;

namespace RM.MailshotsOnline.PCL.Services.Reporting
{
    public interface IDateRangeReportGenerator<out T>
    {
        T Generate(DateTime start, DateTime end);
    }
}
