using HC.RM.Common.Azure.Extensions;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
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

        public ProofPdfController(ISparqQueueService sparqQueueService, IMailshotsService mailshotsService, IMembershipService membershipService)
            : base(membershipService)
        {
            _mailshotsService = mailshotsService;
            _sparqQueueService = sparqQueueService;
            //TODO: Get a real authentication mechanism in place
            // Encrypt Signature and Validate Signature
            _authenticationKey = "SOMETHING"; 
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
                _log.Error(this.GetType().Name, "Get", "Unauthenticated request for Proof PDF for mailshot {0}.", id);
                return authResult;
            }

            // Confirm that the mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _log.Info(this.GetType().Name, "Get", "Request for Proof PDF for unknown mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            // Confirm that the user owns the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _log.Error(this.GetType().Name, "Get", "Unauthorized request for Proof PDF for mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            HttpResponseMessage result;

            // Generate response based on status of proof PDF
            switch (mailshot.ProofPdfStatus)
            {
                case PCL.Enums.PdfRenderStatus.Complete:
                    // TODO: include shared access signature
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
            // Confirm the user is authenticated
            var authResult = Authenticate();
            if (authResult != null)
            {
                _log.Error(this.GetType().Name, "CreateProof", "Unauthenticated request to create Proof PDF for mailshot {0}.", id);
                return authResult;
            }

            // Confirm that the mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _log.Info(this.GetType().Name, "CreateProof", "Request to create Proof PDF for unknown mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            // Confirm that the user owns the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _log.Error(this.GetType().Name, "CreateProof", "Unauthorized request to create Proof PDF for mailshot {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            // Check the status of the proof process
            if (mailshot.ProofPdfStatus != PCL.Enums.PdfRenderStatus.Pending)
            {
                // Use the Sparq service to create a render PDF job
                bool messageSent = false;
                try
                {
                    messageSent = await _sparqQueueService.SendRenderJob(mailshot);
                    _log.Info(this.GetType().Name, "CreateProof", "Proof PDF requested for mailshot {0}", id);
                }
                catch (Exception ex)
                {
                    _log.Error(this.GetType().Name, "CreateProof", "Error when attempting to send render job for mailshot {0}: {1}", id, ex.Message);
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
                    _log.Error(this.GetType().Name, "CreateProof", "Unable to update status of mailshot object {0}: {1}", id, ex.Message);
                }

                return result;
            }
            else
            {
                _log.Info(this.GetType().Name, "CreateProof", "Request to create Proof PDF for mailshot {0}: Proof PDF is already pending.", id);
                return ErrorMessage(HttpStatusCode.Conflict, "There is already a proof PDF generation job pending for this mailshot.  Please wait for this job to complete before starting another.");
            }
        }

        [HttpPost]
        public HttpResponseMessage ProofReady(Guid id, string orderResults)
        {
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            // TEMP:
            // Save the orderResults string to the mailshot for analysis
            mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Complete;
            mailshot.ProofPdfUrl = orderResults;
            _mailshotsService.SaveMailshot(mailshot);

            /*
            //TODO: Authenticate that the response is valid
            if (data.AuthenticationKey != _authenticationKey)
            {
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            // Validate that we have a proof PDF in the request
            if (!data.Results.Any())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                //TODO: Confirm how this process should work with Mahen
                if (data.Results.First().Status != "success")
                {
                    //TODO: Update the mailshot with an error status??
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            */

            //TODO: Download the proof PDF from the private blob storage
            //TODO: Save the proof PDF to the public blob storage
            //TODO: Update the Mailshot with the appropriate data
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}