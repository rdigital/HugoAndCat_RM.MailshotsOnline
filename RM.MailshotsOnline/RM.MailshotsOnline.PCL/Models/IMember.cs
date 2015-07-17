using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMember
    {
        int Id { get; set; }

        string EmailAddress { get; set; }

        string Title { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        bool IsApproved { get; set; }

        bool IsLockedOut { get; set; }
    }
}
