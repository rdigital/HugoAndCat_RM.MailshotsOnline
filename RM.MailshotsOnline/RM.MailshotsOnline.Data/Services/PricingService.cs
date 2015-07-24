using Microsoft.Azure;
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

        /// <summary>
        /// Creates a new instance of the PricingService class
        /// </summary>
        public PricingService()
        {
            _context = new StorageContext();
        }

        /// <summary>
        /// Creates a new instance of the PricingService class, specifying the connection string name to pass to the Storage Context.
        /// Added to allow unit tests to specify the connection string properly.
        /// </summary>
        /// <param name="connectionStringName">Connection string name</param>
        public PricingService(string connectionStringName)
        {
            _context = new StorageContext(connectionStringName);
        }

        /// <summary>
        /// Gets the Postal Options
        /// </summary>
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
