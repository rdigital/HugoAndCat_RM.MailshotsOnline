using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.Entities.DataModels;

namespace RM.MailshotsOnline.Test.Mocks
{
    public class MockPricingService : IPricingService
    {
        public IEnumerable<IPostalOption> GetPostalOptions(int formatId = 0)
        {
            var result = new List<PostalOption>();
            result.Add(new PostalOption() { PostalOptionId = Guid.NewGuid(), Currency = "GBP", FormatId = 1, Name = "First class", PricePerUnit = 0.63M, Tax = 0.12M, TaxCode = "V" });
            result.Add(new PostalOption() { PostalOptionId = Guid.NewGuid(), Currency = "GBP", FormatId = 1, Name = "Second class", PricePerUnit = 0.54M, Tax = 0.10M, TaxCode = "V" });
            result.Add(new PostalOption() { PostalOptionId = Guid.NewGuid(), Currency = "GBP", FormatId = 2, Name = "First class", PricePerUnit = 0.70M, Tax = 0.14M, TaxCode = "V" });
            result.Add(new PostalOption() { PostalOptionId = Guid.NewGuid(), Currency = "GBP", FormatId = 2, Name = "Second class", PricePerUnit = 0.60M, Tax = 0.12M, TaxCode = "V" });

            if (formatId > 0)
            {
                return result.Where(p => p.FormatId == formatId);
            }

            return result;
        }
    }
}
