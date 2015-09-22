using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Models.Reporting;

namespace RM.MailshotsOnline.Entities.DataModels.Reports
{
    public abstract class Report : IReport
    {
        public abstract DateTime CreatedDate { get; set; }

        public abstract string Name { get; }
    }
}
