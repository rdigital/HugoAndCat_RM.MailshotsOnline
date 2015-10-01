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
    public class ReportsController : UmbracoApiController
    {
        private static IReportingService _reportingService;
        private static IBlobService _blobService;
        private static IFtpService _ftpService;

        public ReportsController(IReportingService reportingService, IBlobService blobService, IFtpService ftpService)
        {
            _reportingService = reportingService;
            _blobService = blobService;
            _ftpService = ftpService;
        }

        [HttpPost]
        [RequireHttps]
        public HttpResponseMessage GenerateReport(string type, string token)
        {
            IReport report;
            byte[] reportBytes;
            string sftpStuffBla;

            switch (type.ToLower())
            {
                case "membership":

                    report = _reportingService.MembershipReportGenerator.Generate();
                    reportBytes = CreateCsv(report, ((IMembershipReport) report).Members);

                    // set sftp details


                    break;

                case "transactions":

                    report = _reportingService.TransactionsReportGenerator.Generate();
                    reportBytes = CreateCsv(report, ((ITransactionsReport) report).Transactions);

                    // set sftp details


                    break;

                default:

                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            // sftp
            // bla();

            // store blob
            _blobService.StoreAsync(reportBytes, report.Name, "Report (CSV)");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private byte[] CreateCsv(IReport report, IEnumerable data)
        {
            if (report == null || data == null)
            {
                return null;
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

            return csvBytes;
        }
    }
}