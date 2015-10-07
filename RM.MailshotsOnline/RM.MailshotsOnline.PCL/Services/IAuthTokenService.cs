using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IAuthTokenService
    {
        IAuthToken Create(string serviceName);

        bool Consume(string serviceName, string id);
    }
}
