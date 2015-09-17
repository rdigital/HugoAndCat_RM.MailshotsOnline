using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class CaptureDetailsSurfaceController : BaseSurfaceController
    {
        private readonly ICampaignService _campaignService;
        private readonly IPricingService _pricingService;
        private readonly IInvoiceService _invoiceService;

        public CaptureDetailsSurfaceController(IMembershipService membershipService, ILogger logger, ICampaignService campaignService, IPricingService pricingService, IInvoiceService invoiceService) 
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _pricingService = pricingService;
            _invoiceService = invoiceService;
        }

        [ChildActionOnly]
        public ActionResult ShowDetailsForm(CaptureDetails model)
        {
            var viewModel = new CaptureDetailsViewModel()
            {
                PageModel = model,
                BuildingName = LoggedInMember.BuildingName,
                BuildingNumber = LoggedInMember.BuildingNumber,
                OrganisationName = LoggedInMember.OrganisationName,
                Country = LoggedInMember.Country,
                FlatNumber = LoggedInMember.FlatNumber,
                JobTitle = LoggedInMember.JobTitle,
                Mobile = LoggedInMember.MobilePhoneNumber,
                Postcode = LoggedInMember.Postcode,
                StreetAddress1 = LoggedInMember.Address1,
                StreetAddress2 = LoggedInMember.Address2,
                Town = LoggedInMember.City,
                WorkPhone = LoggedInMember.WorkPhoneNumber,
                CampaignId = Request.QueryString["campaignId"]
            };

            return PartialView("~/Views/CaptureDetails/Partials/CaptureDetailsForm.cshtml", viewModel);
        }

        public ActionResult CaptureDetails(CaptureDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Update the user's details
            LoggedInMember.OrganisationName = viewModel.OrganisationName;
            LoggedInMember.JobTitle = viewModel.JobTitle;
            LoggedInMember.FlatNumber = viewModel.FlatNumber;
            LoggedInMember.BuildingNumber = viewModel.BuildingNumber;
            LoggedInMember.BuildingName = viewModel.BuildingName;
            LoggedInMember.Address1 = viewModel.StreetAddress1;
            LoggedInMember.Address2 = viewModel.StreetAddress2;
            LoggedInMember.City = viewModel.Town;
            LoggedInMember.Postcode = viewModel.Postcode;
            LoggedInMember.Country = viewModel.Country;
            LoggedInMember.WorkPhoneNumber = viewModel.WorkPhone;
            LoggedInMember.MobilePhoneNumber = viewModel.Mobile;

            bool updated = false;
            try
            {
                updated = MembershipService.Save(LoggedInMember.EmailAddress, LoggedInMember);
                Log.Info(this.GetType().Name, "CaptureDetails", "Organisation details updated for user with email address {0}.", LoggedInMember.EmailAddress);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "CaptureDetails", ex);
                Log.Error(this.GetType().Name, "CaptureDetails", "Unable to update the user's details.");
            }

            if (!updated)
            {
                ModelState.AddModelError("CampaignId", "We were unable to save your details to the database.");
                return CurrentUmbracoPage();
            }

            // Fetch the campaign
            ICampaign campaign = null;
            Guid campaignId = Guid.Empty;
            if (Guid.TryParse(viewModel.CampaignId, out campaignId))
            {
                try
                {
                    campaign = _campaignService.GetCampaign(campaignId);
                }
                catch (Exception ex)
                {
                    Log.Exception(this.GetType().Name, "CaptureDetails", ex);
                }
            }

            if (campaign == null)
            {
                Log.Error(this.GetType().Name, "CaptureDetails", "Unable to get campaign with ID {0}.", viewModel.CampaignId);
                ModelState.AddModelError("CampaignId", "We were unable to get the campaign details.");
                return CurrentUmbracoPage();
            }

            if (campaign.UserId != LoggedInMember.Id)
            {
                Log.Error(this.GetType().Name, "CaptureDetails", "Unauthorised attempt to access campaign with ID {0}.", viewModel.CampaignId);
                ModelState.AddModelError("CampaignId", "We were unable to get the campaign details.");
                return CurrentUmbracoPage();
            }

            // Fetch the invoice
            var campaignInvoices = _invoiceService.GetInvoicesForCampaign(campaign);
            if (campaignInvoices == null || !campaignInvoices.Any())
            {
                Log.Warn(this.GetType().Name, "CaptureDetails", "No invoices found for campaign ID {0}", campaignId);
                ModelState.AddModelError("CampaignId", "We were unable to get the campaign details.");
                return CurrentUmbracoPage();
            }

            var invoice = campaignInvoices.OrderByDescending(i => i.UpdatedDate).FirstOrDefault(i => i.Status == PCL.Enums.InvoiceStatus.Draft && !string.IsNullOrEmpty(i.PaypalApprovalUrl));
            if (invoice == null)
            {
                Log.Warn(this.GetType().Name, "CaptureDetails", "No draft invoice found for campaign ID {0}", campaignId);
                ModelState.AddModelError("CampaignId", "We were unable to get the campaign details.");
                return CurrentUmbracoPage();
            }

            invoice.Status = PCL.Enums.InvoiceStatus.Submitted;
            _invoiceService.Save(invoice);

            // Redirect to the paypal page
            return Redirect(invoice.PaypalApprovalUrl);
        }
    }
}
