using System;

namespace RM.MailshotsOnline.PCL.Models.Reporting
{
    public interface IReport
    {
        DateTime CreatedDate { get; set; }

        string Name { get; }
    }
}