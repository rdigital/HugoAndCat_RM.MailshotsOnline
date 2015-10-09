using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace RM.MailshotsOnline.PCL.Models.Reporting
{
    public interface IReport
    {
        DateTime CreatedDate { get; set; }

        string Name { get; }
    }
}