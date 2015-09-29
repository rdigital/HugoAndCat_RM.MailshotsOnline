using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class OrderDetailSurfaceController : BaseSurfaceController
    {
        private ICampaignService _campaignService;
        private IInvoiceService _invoiceService;

        public OrderDetailSurfaceController (IMembershipService membershipService, ILogger logger, ICampaignService campaignService, IInvoiceService invoiceService)
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _invoiceService = invoiceService;
        }

        // GET: OrderDetailSurface
        public ActionResult ShowCancelButton(ICampaign campaign)
        {
            var viewModel = new CancelOrderViewModel() { CampaignId = campaign.CampaignId };
            return PartialView("~/Views/Orders/Partials/CancelButton.cshtml", viewModel);
        }

        public ActionResult CancelOrder(CancelOrderViewModel viewModel)
        {
            ICampaign campaign = null;
            try
            {
                campaign = _campaignService.GetCampaignWithInvoices(viewModel.CampaignId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "CancelOrder", ex);
                Log.Error(this.GetType().Name, "CancelOrder", "Error getting campaign with ID {0}.", viewModel.CampaignId);
            }

            if (campaign == null)
            {
                ViewBag.Error = "Unable to get campaign to cancel";
                return CurrentUmbracoPage();
            }

            // Confirm that the user owns the campaign
            if (campaign.UserId != LoggedInMember.Id)
            {
                ViewBag.Error = "Unable to get campaign to cancel";
                return CurrentUmbracoPage();
            }

            var invoiceHelper = new InvoiceHelper(_campaignService, _invoiceService);
            List<string> errorMessages;
            var success = invoiceHelper.CancelInvoice(campaign, out errorMessages);

            if (!success)
            {
                var errorMessage = "There was an error cancelling the campaign";
                if (errorMessages != null)
                {
                    errorMessage += ": " + errorMessages.FirstOrDefault();
                }

                ViewBag.Error = errorMessage;
                return CurrentUmbracoPage();
            }

            if (Request.QueryString["campaignId"] != null)
            {
                return RedirectToCurrentUmbracoPage("campaignId=" + Request.QueryString["campaignId"]);
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}