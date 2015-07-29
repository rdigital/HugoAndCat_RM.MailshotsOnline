using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            _authenticationKey = "SOMETHING"; 
        }

        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.ErrorCode, ex.Message);
            }

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Forbidden");
            }

            HttpResponseMessage result;

            switch (mailshot.ProofPdfStatus)
            {
                case PCL.Enums.PdfRenderStatus.Complete:
                    result = Request.CreateResponse(HttpStatusCode.OK, mailshot.ProofPdfUrl);
                    break;
                case PCL.Enums.PdfRenderStatus.Pending:
                    result = Request.CreateResponse(HttpStatusCode.PreconditionFailed, "The PDF is not ready.");
                    break;
                case PCL.Enums.PdfRenderStatus.None:
                default:
                    result = Request.CreateResponse(HttpStatusCode.NotFound, "No proof PDF has been requested for this mailshot.");
                    break;
            }

            return result;
        }

        [HttpPost]
        public HttpResponseMessage CreateProof(Guid id)
        {
            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.ErrorCode, ex.Message);
            }

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Forbidden");
            }

            if (mailshot.ProofPdfStatus != PCL.Enums.PdfRenderStatus.Pending)
            {
                // Use the Sparq service to create a render PDF job
                var messageSent = _sparqQueueService.SendRenderJob(mailshot);

                if (messageSent)
                {
                    // Update the Mailshot so that the status can be checked
                    mailshot.ProofPdfStatus = PCL.Enums.PdfRenderStatus.Pending;
                    _mailshotsService.SaveMailshot(mailshot);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Could not start the proof PDF creation process");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "There is already a proof PDF generation job pending for this mailshot.");
            }
        }

        [HttpPost]
        public HttpResponseMessage ProofReady(Guid id, PdfRenderJobViewModel data)
        {
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            //TODO: Authenticate that the response is valid
            if (data.AuthenticationKey != _authenticationKey)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
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