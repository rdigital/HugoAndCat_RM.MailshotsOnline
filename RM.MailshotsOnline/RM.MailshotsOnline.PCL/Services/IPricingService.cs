using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IPricingService
    {
        IEnumerable<IPostalOption> GetPostalOptions(int formatId = 0);
    }
}
