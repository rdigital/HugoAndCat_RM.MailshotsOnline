using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.ViewModels;
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

        /// <summary>
        /// Gets all lists for the currently logged in user.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all rented lists for the currently logged in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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

        /// <summary>
        /// Checks to see if the supplied list name is unique for the currently logged in user.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCheckListNameIsUnique(string listName)
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            bool nameIsNotUnique =
                _dataService.GetDistributionListsForUser(_loggedInMember.Id).Any(d => string.Equals(d.Name, listName, StringComparison.CurrentCultureIgnoreCase));

            if (nameIsNotUnique)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "List name is not unique.", statusCode = HttpStatusCode.BadRequest });
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostUploadCsv(ModifyListUploadFileModel model)
        {

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
