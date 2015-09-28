using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class DistributionListController: ApiBaseController
    {
        private readonly IDataService _dataService;

        public DistributionListController(IMembershipService membershipService, ILogger logger, IDataService dataService) : base(membershipService, logger)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public HttpResponseMessage GetMyLists()
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            var results = _dataService.GetDistributionListsForUser(_loggedInMember.Id);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        public HttpResponseMessage GetMyRentedLists()
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            var results = _dataService.GetDistributionLists(l => l.UserId == -1);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}
