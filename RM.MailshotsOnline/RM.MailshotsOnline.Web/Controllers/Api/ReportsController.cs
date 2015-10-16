using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Collections;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;
using System.Threading.Tasks;
using HC.RM.Common.Azure;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ReportsController : ApiBaseController
    {
        private static IReportingService _reportingService;
        private static IReportingBlobService _blobService;
        private static IReportingSftpService _sftpService;
        private static IAuthTokenService _authTokenService;

        // hacked in logger
        private static ILogger _log = new Logger();

        public ReportsController(ILogger logger, IReportingService reportingService, IReportingBlobService blobService,
            IReportingSftpService sftpService, IAuthTokenService authTokenService)
            : base(logger)
        {
            _reportingService = reportingService;
            _blobService = blobService;
            _sftpService = sftpService;
            _authTokenService = authTokenService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GenerateReport(AuthTokenPostModel tokenPostModel)
        {
            if (string.IsNullOrEmpty(tokenPostModel.Type) || string.IsNullOrEmpty(tokenPostModel.Token))
            {
                var message = "Method was called with bad parameters";

                _log.Error(this.GetType().Name, "GenerateReport", message);
                return ErrorMessageDebug(HttpStatusCode.NotAcceptable, message);    //todo: change this back to 400
            }

            if (!_authTokenService.Consume(tokenPostModel.Service, tokenPostModel.Token))
            {
                var message = "Method was called with an invalid token";

                _log.Error(this.GetType().Name, "GenerateReport", message);
                return ErrorMessageDebug(HttpStatusCode.Unauthorized, message); //todo: change this back to 400
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

                    return ErrorMessageDebug(HttpStatusCode.Ambiguous, "Method was called with an invalid report type"); //todo: change this back to 400
            }

            if (data != null)
            {
                using (var m = new MemoryStream())
                using (var streamWriter = new StreamWriter(m))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(data);
                    streamWriter.Flush();
                    m.Flush();

                    var filename = report.Name + ".csv";

                    bool fileTransferSuccess = false;
                    bool blobStoreageSuccess = false;

                    try
                    {
                        fileTransferSuccess = _sftpService.Put(m, $"{Constants.Reporting.SftpDirectory}/{filename}");

                        if (!fileTransferSuccess)
                        {
                            throw new Exception("SFTP service could not transfer the file.");
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error(this.GetType().Name, "GenerateReport", "Upload to SFTP server failed!", e);
                    }

                    try
                    {
                        var blobName = await _blobService.StoreAsync(m.ToArray(), filename, "text/csv");

                        if (string.IsNullOrEmpty(blobName))
                        {
                            throw new Exception(
                                "The blob service did not return a blob name after attempting to store the report.");
                        }

                        blobStoreageSuccess = true;
                    }
                    catch (Exception e)
                    {
                        _log.Error(this.GetType().Name, "GenerateReport", "Upload to blob store failed!", e);
                    }

                    _log.Info(this.GetType().Name, "GenerateReport",
                        $"Report generation complete! SFTP success: {fileTransferSuccess}, blob storage success: {blobStoreageSuccess}");

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }

            _log.Error(this.GetType().Name, "GenerateReport", "Data was null - exiting");
            return ErrorMessageDebug(HttpStatusCode.InternalServerError, "Data was null - exiting");
        }
    }
}