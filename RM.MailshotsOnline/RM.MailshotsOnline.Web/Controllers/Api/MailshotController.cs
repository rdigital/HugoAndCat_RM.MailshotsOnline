using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    [MemberAuthorize()]
    public class MailshotController : ApiBaseController
    {
        private IMailshotsService _mailshotsService;

        public MailshotController(IMailshotsService mailshotsService, IMembershipService membershipService)
            : base(membershipService)
        {
            _mailshotsService = mailshotsService;
        }

        // GET: Mailshot
        public IEnumerable<IMailshot> GetAllMailshots(int userId)
        {
            Authenticate();

            if (loggedInMember.Id != userId)
            {
                throw new HttpException(403, "You cannot access another user's information.");
            }

            return _mailshotsService.GetUsersMailshots(userId);
        }
    }
}