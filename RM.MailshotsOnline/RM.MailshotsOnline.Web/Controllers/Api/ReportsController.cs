using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Collections;
using System.IO;
using CsvHelper;
using RM.MailshotsOnline.Data.Services.Reporting;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ReportsController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetReport(string type)
        {
            // Check to see if the user is logged into the front-end site
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            switch (type.ToLower())
            {
                case "membership":
                    return CreateCsvResponse(ReportingService.MembershipReportGenerator.Generate()?.Members);
                case "transactions":
                    return CreateCsvResponse(ReportingService.TransactionsReportGenerator.Generate()?.Transactions);
                default:
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        private HttpResponseMessage CreateCsvResponse(IEnumerable e)
        {
            if (e == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            byte[] csvBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(e);
                }

                csvBytes = memoryStream.ToArray();
            }

            if (csvBytes.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(csvBytes);
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
            result.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(3600),
                NoCache = false
            };

            return result;
        }
    }
}