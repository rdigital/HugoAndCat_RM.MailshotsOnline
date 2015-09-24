using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Collections;
using System.IO;
using System.Net.Http.Headers;
using CsvHelper;
using RM.MailshotsOnline.Data.Services.Reporting;
using RM.MailshotsOnline.Entities.DataModels.Reports;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services.Reporting;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ReportsController : UmbracoAuthorizedApiController
    {
        private static IReportingService _reportingService;

        public ReportsController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet]
        public HttpResponseMessage GetReport(string type)
        {
            // Check to see if the user is logged into the front-end site
            //var authResult = Authenticate();
            //if (authResult != null)
            //{
            //    return authResult;
            //}

            switch (type.ToLower())
            {
                case "membership":

                    var membershipReport = _reportingService.MembershipReportGenerator.Generate();
                    return CreateCsvResponse(membershipReport, membershipReport.Members);

                case "transactions":

                    var transactionsReport = _reportingService.TransactionsReportGenerator.Generate();
                    return CreateCsvResponse(transactionsReport, transactionsReport.Transactions);

                default:

                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        private HttpResponseMessage CreateCsvResponse(IReport report, IEnumerable data)
        {
            if (report == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            byte[] csvBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(data);
                }

                csvBytes = memoryStream.ToArray();
            }

            if (csvBytes.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(csvBytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = $"{report.Name} - {report.CreatedDate.ToString("yyyyMMddHHmmss")}.csv",
                    Size = csvBytes.Length,
                    CreationDate = report.CreatedDate
                };
            result.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(3600),
                NoCache = false
            };

            return result;
        }
    }
}