using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Collections;
using System.IO;
using System.Net.Http.Headers;
using CsvHelper;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Persistence;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Services.Reporting;
using RM.MailshotsOnline.Entities.DataModels.Reports;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class TokenAuthController : UmbracoApiController
    {
        [HttpPost]
        public HttpResponseMessage SetToken(string id)
        {
            // take the token (id) and save it to the db

            // return ok else 500 or something.

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}