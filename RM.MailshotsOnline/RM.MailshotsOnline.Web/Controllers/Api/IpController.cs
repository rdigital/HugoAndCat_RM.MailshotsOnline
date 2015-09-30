using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class IpController : UmbracoApiController
    {
        private readonly ICryptographicService _cryptographicService;

        public IpController(ICryptographicService cryptographicService)
        {
            _cryptographicService = cryptographicService;
        }

        [HttpGet]
        public HttpResponseMessage GetIp(string token)
        {
            if (token != GetTodaysValidToken())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return Request.CreateResponse(HttpStatusCode.OK, HttpContext.Current.Request.UserHostAddress);
        }

        private string GetTodaysValidToken()
        {
            return _cryptographicService.Encrypt(Constants.Reports.Key, DateTime.UtcNow.ToString("yyyyMMMMdd"));
        }
    }
}