using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common.Azure.Helpers;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Data.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private StorageContext _context;
        private static ILogger _logger;

        public AuthTokenService(ILogger logger)
            : this(new StorageContext())
        {
            _logger = logger;
        }

        public AuthTokenService(StorageContext storageContext)
        {
            _context = storageContext;
        }

        public IAuthToken Create(string serviceName)
        {
            var token = _context.AuthTokens.Add(new AuthToken
            {
                AuthTokenId = Guid.NewGuid(),
                ServiceName = serviceName,
                CreatedUtc = DateTime.UtcNow
            });

            _context.SaveChanges();

            return token;
        }

        public bool Consume(string serviceName, string token)
        {
            try
            {
                var match = _context.AuthTokens
                    .AsEnumerable()
                    .FirstOrDefault(x => x.AuthTokenId.ToString().Equals(token) && x.ServiceName.Equals(serviceName));

                if (match != null)
                {
                    _context.AuthTokens.Remove(match);
                    _context.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.Error(GetType().Name, "Consume", e.Message, e);
            }

            return false;
        }
    }
}
