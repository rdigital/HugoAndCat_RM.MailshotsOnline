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
        public IPostalOption GetPostalOption(Guid postalOptionId)
        {
            throw new NotImplementedException();
        }

        public IPostalOption GetPostalOptionByUmbracoId(int umbracoId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPostalOption> GetPostalOptions()
        {
            var result = new List<PostalOption>();
            result.Add(new PostalOption() { PostalOptionId = Guid.NewGuid(), Currency = "GBP", UmbracoId = 1000, Name = "First class", PricePerUnit = 0.63M, Tax = 0.12M, TaxCode = "V" });
            result.Add(new PostalOption() { PostalOptionId = Guid.NewGuid(), Currency = "GBP", UmbracoId = 1001, Name = "Second class", PricePerUnit = 0.54M, Tax = 0.10M, TaxCode = "V" });

            return result;
        }

        public IPostalOption SavePostalOption(IPostalOption postalOption)
        {
            throw new NotImplementedException();
        }
    }
}
