using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;

namespace RM.MailshotsOnline.Test.Helpers
{
    public class UmbracoHelper : BaseRoutingTest
    {
        public UmbracoContext GetMockContext()
        {
            var routingContext = GetRoutingContext("/test");
            return routingContext.UmbracoContext;
        }
    }
}
