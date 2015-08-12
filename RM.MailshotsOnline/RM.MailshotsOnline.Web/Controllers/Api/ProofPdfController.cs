using HC.RM.Common.Azure.Extensions;
using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.Orders;
using HC.RM.Common.PCL.Persistence;
using Newtonsoft.Json;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ProofPdfController : ApiBaseController
    {
        private IMailshotsService _mailshotsService;

        private ISparqQueueService _sparqQueueService;

        private string _authenticationKey;

        private string _controllerName;

        public ProofPdfController(ISparqQueueService sparqQueueService, IMailshotsService mailshotsService, IMembershipService membershipService)
            : base(membershipService)
        {
            _mailshotsService = mailshotsService;
            _sparqQueueService = sparqQueueService;
            //TODO: Get a real authentication mechanism in place
            // Encrypt Signature and Validate Signature
            _authenticationKey = "SOMETHING";
            _controllerName = this.GetType().Name;
        }

        /// <summary>
        /// Gets the proof PDF for a given mailshot, it it has been generated
        /// </summary>
        /// <param name="id">ID of the mailshot</param>
        /// <returns>HTTP OK and URL of proof PDF if it has been generated</returns>
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            // Confirm the user is authenticated
            var authResult = Authenticate();
            if (authResult != null)
            {
                _log.Error(_controllerName, "Get", "Unauthenticated request for Proof PDF for mailshot {0}.", id);
                return authResult;
            }

            // Confirm that the mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _log.Info(_controllerName, "Get", "Request for Proof PDF for unknown mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            // Confirm that the user owns the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _log.Error(_controllerName, "Get", "Unauthorized request for Proof PDF for mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            HttpResponseMessage result;

            // Generate response based on status of proof PDF
            switch (mailshot.ProofPdfStatus)
            {
                case PCL.Enums.PdfRenderStatus.Complete:
                    // TODO: include shared access signature?
                    result = Request.CreateResponse(HttpStatusCode.OK, new { url = mailshot.ProofPdfUrl });
                    break;
                case PCL.Enums.PdfRenderStatus.Pending:
                    result = ErrorMessage(HttpStatusCode.PreconditionFailed, "The PDF is not ready.");
                    break;
                case PCL.Enums.PdfRenderStatus.None:
                default:
                    result = ErrorMessage(HttpStatusCode.NotFound, "No proof PDF has been requested for this mailshot.");
                    break;
            }

            return result;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateProof(Guid id)
        {
            string methodName = "CreateProof";

            // Confirm the user is authenticated
            var authResult = Authenticate();
            if (authResult != null)
            {
                _log.Error(_controllerName, methodName, "Unauthenticated request to create Proof PDF for mailshot {0}.", id);
                return authResult;
            }

            // Confirm that the mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _log.Info(_controllerName, methodName, "Request to create Proof PDF for unknown mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            // Confirm that the user owns the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _log.Error(_controllerName, methodName, "Unauthorized request to create Proof PDF for mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            // Check the status of the proof process
            if (mailshot.ProofPdfStatus != PCL.Enums.PdfRenderStatus.Pending)
            {
                // Create order number for the job
                mailshot.ProofPdfOrderNumber = Guid.NewGuid();

                try
                {
                    _mailshotsService.SaveMailshot(mailshot);
                }
                catch (Exception ex)
                {
                    _log.Exception(_controllerName, methodName, ex);
                    _log.Error(_controllerName, methodName, "Unable to save the order number to mailshot {0}: {1}", id, ex.Message);
                }

                // Use the Sparq service to create a render PDF job
                bool messageSent = false;
                try
                {
                    messageSent = await _sparqQueueService.SendRenderJob(mailshot);
                    _log.Info(_controllerName, methodName, "Proof PDF requested for mailshot {0}", id);
                }
                catch (Exception ex)
                {
                    _log.Exception(_controllerName, methodName, ex);
                    _log.Error(_controllerName, methodName, "Error when attempting to send render job for mailshot {0}: {1}", id, ex.Message);
                }

                HttpResponseMessage result = null;
                if (messageSent)
                {
                    // Update the Mailshot so that the status can be checked
                    mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Pending;

                    result = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Failed;

                    result = ErrorMessage(HttpStatusCode.InternalServerError, "Error starting the proof PDF creation process");
                }

                try
                {
                    _mailshotsService.SaveMailshot(mailshot);
                }
                catch (Exception ex)
                {
                    _log.Error(_controllerName, methodName, "Unable to update status of mailshot object {0}: {1}", id, ex.Message);
                }

                return result;
            }
            else
            {
                _log.Info(_controllerName, methodName, "Request to create Proof PDF for mailshot {0}: Proof PDF is already pending.", id);
                return ErrorMessage(HttpStatusCode.Conflict, "There is already a proof PDF generation job pending for this mailshot.  Please wait for this job to complete before starting another.");
            }
        }

        [HttpPost]
        [Route("ProofReady/{id}", Name = "ProofReady")]
        public async Task<HttpResponseMessage> ProofReady(Guid id, OrderResults orderResults)
        {
            string methodName = "ProofReady";

            _log.Info(_controllerName, methodName, "Proof ready post sent for mailshot {0}. Order ID: {1}; Results: {2}", id, orderResults.OrderId, orderResults.Results);

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _log.Warn(_controllerName, methodName, "Proof ready post sent for mailshot {0} which was not found.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            Guid orderNumber;
            if (!Guid.TryParse(orderResults.OrderId, out orderNumber))
            {
                _log.Warn(_controllerName, methodName, "Proof ready post sent for mailshot {0} with incorrect order number {0}.", id, orderResults.OrderId);
                return ErrorMessage(HttpStatusCode.BadRequest, "Invalid order number.");
            }

            if (mailshot.ProofPdfOrderNumber != orderNumber)
            {
                _log.Warn(_controllerName, methodName, "Proof ready post sent for mailshot {0} with incorrect order number {0}.", id, orderResults.OrderId);
                return ErrorMessage(HttpStatusCode.BadRequest, "Mismatch on order number.");
            }

            Collection<SparqOrderResult> parsedResults = null;

            try
            {
                parsedResults = JsonConvert.DeserializeObject<Collection<SparqOrderResult>>(orderResults.Results);
            }
            catch (Exception ex)
            {
                _log.Exception(_controllerName, methodName, ex);
                _log.Warn(_controllerName, methodName, "Proof ready post sent for mailshot {0} without valid results.", id);
                mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Failed;
                _mailshotsService.SaveMailshot(mailshot);
                return ErrorMessage(HttpStatusCode.BadRequest, "Results invalid.");
            }

            // Should only have one result
            var result = parsedResults.FirstOrDefault();
            if (result == null)
            {
                _log.Warn(_controllerName, methodName, "Proof ready post sent for mailshot {0} without valid results.", id);
                mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Failed;
                _mailshotsService.SaveMailshot(mailshot);
                return ErrorMessage(HttpStatusCode.BadRequest, "No results returned.");
            }

            _log.Info(_controllerName, methodName, "Proof for mailshot {0} returned status {1}.", id, result.Status);

            if (result.Errors != null && result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    _log.Warn(_controllerName, methodName, "Error rendering mailshot {0}, order number {1}: {2}", id, orderNumber, error.Message);
                }
            }
            
            if (string.IsNullOrEmpty(result.BlobId))
            {
                _log.Warn(_controllerName, methodName, "Proof ready post sent for mailshot {0} without blob ID.", id);
                mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Failed;
                _mailshotsService.SaveMailshot(mailshot);
                return ErrorMessage(HttpStatusCode.BadRequest, "No blob ID returned.");
            }

            mailshot.ProofPdfBlobId = result.BlobId;
            _mailshotsService.SaveMailshot(mailshot);

            // Download the PDF
            var serviceLayerBlobHelper = new BlobStorageHelper(ConfigHelper.SparqServiceBlobConnectionString, ConfigHelper.SparqServiceBlobContainer);
            byte[] pdfBytes = null;
            try
            {
                pdfBytes = serviceLayerBlobHelper.FetchBytes(result.BlobId);
            }
            catch (Exception ex)
            {
                _log.Exception(_controllerName, methodName, ex);
                _log.Error(_controllerName, methodName, "Unable to download blob {0} from shared service blob storage for mailshot {1}.", result.BlobId, id);
                mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Failed;
                _mailshotsService.SaveMailshot(mailshot);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to download proof PDF from blob storage.");
            }

            // Save it to the public storage
            var appBlobHelper = new BlobStorageHelper(ConfigHelper.StorageConnectionString, ConfigHelper.SparqBlobContainer);
            var blobFilename = string.Format("{2}/{0}/{1:yyyyMMddHHmmssfff}.pdf", mailshot.MailshotId.ToString("D"), DateTime.UtcNow, mailshot.UserId);
            var blobMediaType = "application/pdf";
            string pdfUrl = null;

            try
            {
                pdfUrl = appBlobHelper.StoreBytes(pdfBytes, blobFilename, blobMediaType);
            }
            catch (Exception ex)
            {
                _log.Exception(_controllerName, methodName, ex);
                _log.Error(_controllerName, methodName, "Unable to save PDF to blob storage for mailshot {0}.", id);
                mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Failed;
                _mailshotsService.SaveMailshot(mailshot);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to save proof PDF to blob storage.");
            }

            mailshot.ProofPdfUrl = pdfUrl;
            mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Complete;
            _mailshotsService.SaveMailshot(mailshot);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}