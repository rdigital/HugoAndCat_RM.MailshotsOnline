using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Collections;
using System.IO;
using System.Net.Http.Headers;
using CsvHelper;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Helpers;
using HC.RM.Common.PCL.Persistence;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Helpers;
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
        private static ILogger _logger;
        private static IAuthTokenService _authTokenService;

        public ReportsController(IReportingService reportingService, IBlobService blobService, IFtpService ftpService,
            ILogger logger, IAuthTokenService authTokenService)
        {
            _reportingService = reportingService;
            _blobService = blobService;
            _ftpService = ftpService;
            _logger = logger;
            _authTokenService = authTokenService;
        }

        [HttpPost]
        [RequireHttps]
        public HttpResponseMessage GenerateReport(string type, string token, string service)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(token))
            {
                _logger.Error(this.GetType().Name, "GenerateReport", "Method was called with bad parameters");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            if (!_authTokenService.Consume(service, token))
            {
                _logger.Error(this.GetType().Name, "GenerateReport", "Method was called with bad parameters");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            IReport report;
            MemoryStream reportStream;

            switch (type.ToLower())
            {
                case "membership":

                    report = _reportingService.MembershipReportGenerator.Generate();
                    reportStream = CreateCsv(report, ((IMembershipReport) report).Members);

                    break;

                case "transactions":

                    report = _reportingService.TransactionsReportGenerator.Generate();
                    reportStream = CreateCsv(report, ((ITransactionsReport) report).Transactions);

                    break;

                default:

                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (reportStream.Length > 0)
            {
                var filename = report.Name + ".csv";

                try
                {
                    _ftpService.Put(reportStream, $"{Constants.Reporting.SftpDirectory}/{filename}");
                }
                catch (Exception e)
                {
                    _logger.Error(this.GetType().Name, "GenerateReport", $"Upload to SFTP server failed!", e);
                }

                try
                {
                    _blobService.StoreAsync(reportStream.ToArray(), report.Name, "text/csv");
                }
                catch (Exception e)
                {
                    _logger.Error(this.GetType().Name, "GenerateReport", $"Upload to blob store failed!", e);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            _logger.Error(this.GetType().Name, "GenerateReport", "Stream was empty");

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        private MemoryStream CreateCsv(IReport report, IEnumerable data)
        {
            if (report == null || data == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(data);
                }

                return memoryStream;
            }
        }
    }
}