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

        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            HttpResponseMessage result;

            switch (mailshot.ProofPdfStatus)
            {
                case PCL.Enums.PdfRenderStatus.Complete:
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
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            //TODO: Remove this
            _sparqQueueService = new SparqTemp.SparqQueueService(_mailshotsService);

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
            }

            if (mailshot.ProofPdfStatus != PCL.Enums.PdfRenderStatus.Pending)
            {
                // Use the Sparq service to create a render PDF job
                var messageSent = await _sparqQueueService.SendRenderJob(mailshot);

                if (messageSent)
                {
                    // Update the Mailshot so that the status can be checked
                    // Now handled inside sparq service. Double check
                    //mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Pending;
                    //_mailshotsService.SaveMailshot(mailshot);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return ErrorMessage(HttpStatusCode.InternalServerError, "Error starting the proof PDF creation process");
                }
            }
            else
            {
                return ErrorMessage(HttpStatusCode.Conflict, "There is already a proof PDF generation job pending for this mailshot.  Please wait for this job to complete before starting another.");
            }
        }

        [HttpPost]
        public HttpResponseMessage ProofReady(Guid id, PdfRenderJobViewModel data)
        {
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

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

            //TODO: Download the proof PDF from the private blob storage
            //TODO: Save the proof PDF to the public blob storage
            //TODO: Update the Mailshot with the appropriate data
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}