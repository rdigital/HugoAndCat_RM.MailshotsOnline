using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IAuthToken
    {
        Guid AuthTokenId { get; set; }

        DateTime CreatedUtc { get; set; }

        string ServiceName { get; set; }
    }
}
