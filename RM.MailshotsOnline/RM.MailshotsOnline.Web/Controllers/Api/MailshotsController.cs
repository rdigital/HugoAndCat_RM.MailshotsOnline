using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    //[MemberAuthorize()]
    public class MailshotsController : ApiBaseController
    {
        private IMailshotsService _mailshotsService;

        public MailshotsController(IMailshotsService mailshotsService, IMembershipService membershipService)
            : base(membershipService)
        {
            _mailshotsService = mailshotsService;
        }

        // GET: Mailshot
        public IEnumerable<IMailshot> GetAll()
        {
            Authenticate();

            return _mailshotsService.GetUsersMailshots(_loggedInMember.Id);
        }

        [HttpGet]
        public MailshotViewModel Get(Guid id)
        {
            Authenticate();

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                //return Request.CreateResponse(HttpStatusCode.NotFound, "Mailshot not found.");
                throw new HttpException((int)HttpStatusCode.NotFound, "Mailshot not found.");
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden");
            }

            var result = new MailshotViewModel()
            {
                Content = mailshot.Content.Content,
                MailshotId = mailshot.MailshotId,
                Name = mailshot.Name
            };
            return result;
        }

        [HttpPost]
        public Guid Save(MailshotViewModel mailshot)
        {
            if (mailshot != null)
            {
                Authenticate();

                IMailshot mailshotData = new Mailshot()
                {
                    UpdatedDate = DateTime.UtcNow,
                    UserId = _loggedInMember.Id,
                    Draft = true,
                    Name = mailshot.Name
                };

                mailshotData.Content = new MailshotContent()
                {
                    Content = mailshot.Content
                };

                _mailshotsService.SaveMailshot(mailshotData);

                return mailshotData.MailshotId;
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }
        }

        [HttpPost]
        public HttpResponseMessage Update(Guid mailshotId, MailshotViewModel mailshot)
        {
            Authenticate();

            if (mailshot == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide mailshot data to send.");
            }

            var mailshotData = _mailshotsService.GetMailshot(mailshotId);
            if (mailshotData == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No mailshot found with that ID");
            }

            if (mailshotData.UserId != _loggedInMember.Id)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            mailshotData.Name = mailshot.Name;
            mailshotData.Content.Content = mailshot.Content;
            mailshotData.UpdatedDate = DateTime.UtcNow;

            _mailshotsService.SaveMailshot(mailshotData);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public string GetProofPdf(Guid id)
        {
            return "This will be the URL";
        }
    }
}