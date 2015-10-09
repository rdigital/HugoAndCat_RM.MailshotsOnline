using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Collections;
using System.IO;
using CsvHelper;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ReportsController : ApiBaseController
    {
        private static IReportingService _reportingService;
        private static IReportingBlobService _blobService;
        private static IReportingSftpService _sftpService;
        private static IAuthTokenService _authTokenService;

        public ReportsController(IReportingService reportingService, IReportingBlobService blobService, IReportingSftpService sftpService, IAuthTokenService authTokenService)
        {
            _reportingService = reportingService;
            _blobService = blobService;
            _sftpService = sftpService;
            _authTokenService = authTokenService;
        }

        [HttpPost]
        public HttpResponseMessage GenerateReport(AuthTokenPostModel tokenPostModel)
        {
            if (string.IsNullOrEmpty(tokenPostModel.Type) || string.IsNullOrEmpty(tokenPostModel.Token))
            {
                var message = "Method was called with bad parameters";

                _logger.Error(this.GetType().Name, "GenerateReport", message);
                return ErrorMessageDebug(HttpStatusCode.BadRequest, message);
            }

            if (!_authTokenService.Consume(tokenPostModel.Service, tokenPostModel.Token))
            {
                var message = "Method was called with an invalid token";

                _logger.Error(this.GetType().Name, "GenerateReport", message);
                return ErrorMessageDebug(HttpStatusCode.BadRequest, message);
            }

            IReport report;
            IEnumerable data;

            switch (tokenPostModel.Type.ToLower())
            {
                case "membership":

                    report = _reportingService.MembershipReportGenerator.Generate();
                    data = ((IMembershipReport) report).Members;

                    break;

                case "transactions":

                    report = _reportingService.TransactionsReportGenerator.Generate();
                    data = ((ITransactionsReport)report).Transactions;

                    break;

                default:

                    return ErrorMessageDebug(HttpStatusCode.BadRequest, "Method was called with an invalid report type");
            }

            if (data != null)
            {
                using (var m = new MemoryStream())
                using (var streamWriter = new StreamWriter(m))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(data);

                    var filename = report.Name + ".csv";

                    try
                    {
                        var success = _sftpService.Put(m, $"{Constants.Reporting.SftpDirectory}/{filename}");

                        if (!success)
                        {
                            throw new Exception("SFTP service not transfer the file.");
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error(this.GetType().Name, "GenerateReport", "Upload to SFTP server failed!", e);
                    }

                    try
                    {
                        var blobName = _blobService.Store(m.ToArray(), filename, "text/csv");

                        if (!string.IsNullOrEmpty(blobName))
                        {
                            throw new Exception("The blob service did not return a blob name after attempting to store the report.");
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error(this.GetType().Name, "GenerateReport", "Upload to blob store failed!", e);
                    }

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }

            _logger.Error(this.GetType().Name, "GenerateReport", "Stream was empty");
            return ErrorMessageDebug(HttpStatusCode.InternalServerError, "Stream was empty");
        }

        public class AuthTokenPostModel
        {
            public string Type { get; set; }

            public string Token { get; set; }

            public string Service { get; set; }
        }
    }
}