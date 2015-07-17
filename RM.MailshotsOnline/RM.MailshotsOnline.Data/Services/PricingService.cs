using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class PricingService : IPricingService
    {
        private StorageContext _context;

        public PricingService()
        {
            _context = new StorageContext();
        }

        public IEnumerable<IPostalOption> GetPostalOptions(int formatId = 0)
        {
            if (formatId > 0)
            {
                return _context.PostalOptions.Where(p => p.FormatId == formatId);
            }
            else
            {
                return _context.PostalOptions.AsEnumerable();
            }
        }
    }
}
