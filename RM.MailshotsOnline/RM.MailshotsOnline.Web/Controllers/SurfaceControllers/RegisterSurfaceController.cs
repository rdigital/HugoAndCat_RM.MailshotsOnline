using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class RegisterSurfaceController : SurfaceController
    {
        private MembershipService _membershipService = new MembershipService();

        // GET: Register
        [ChildActionOnly]
        public ActionResult ShowRegisterForm(Register model)
        {
            // todo: get valid titles.

            var viewModel = new RegisterViewModel() {PageModel = model};

            return PartialView("~/Views/Partials/Register.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult RegisterForm(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (Members.GetByEmail(model.Email) == null)
            {
                //todo: check password meets requirements

                _membershipService.CreateMember(new Member()
                {
                    EmailAddress = model.Email,
                    Title = model.Title,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsApproved = false,
                    IsLockedOut = false,
                }, model.Password);

                return Redirect("/?registered=true");
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}