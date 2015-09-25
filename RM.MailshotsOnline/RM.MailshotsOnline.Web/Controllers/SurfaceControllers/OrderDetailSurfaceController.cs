using HC.RM.Common.PayPal.Models;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPalService = HC.RM.Common.PayPal.Service;

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

            // Confirm that the appropriate invoice is in a state that can be cancelled.
            var cancelableCampaignStatuses = new List<PCL.Enums.CampaignStatus>()
            {
                PCL.Enums.CampaignStatus.PendingModeration
            };

            if (!cancelableCampaignStatuses.Contains(campaign.Status))
            {
                ViewBag.Error = "Campaign cannot be cancelled";
                return CurrentUmbracoPage();
            }

            // Get the invoice
            var invoice = campaign.LatestInvoice();
            if (invoice == null)
            {
                ViewBag.Error = "No invoice to be cancelled";
                return CurrentUmbracoPage();
            }

            // Check the invoice can be cancelled
            var cancelableInvoiceStatuses = new List<PCL.Enums.InvoiceStatus>()
            {
                PCL.Enums.InvoiceStatus.Submitted,
                PCL.Enums.InvoiceStatus.Processing,
                PCL.Enums.InvoiceStatus.Draft
            };

            if (!cancelableInvoiceStatuses.Contains(invoice.Status))
            {
                ViewBag.Error = "The invoice cannot be cancelled.";
                return CurrentUmbracoPage();
            }

            // Cancel PayPal Order
            if (!string.IsNullOrEmpty(invoice.PaypalOrderId))
            {
                // Void PayPal order
                var paypalService = new PayPalService();
                Order order = null;
                try
                {
                    order = paypalService.GetOrder(invoice.PaypalOrderId);
                    paypalService.VoidOrder(order);
                }
                catch (Exception ex)
                {
                    Log.Exception(this.GetType().Name, "Cancel Order", ex);
                    Log.Error(this.GetType().Name, "Cancel Order", "Unable to cancel PayPal order {0}.", invoice.PaypalOrderId);
                }
            }

            // Update invoice
            invoice.Status = PCL.Enums.InvoiceStatus.Cancelled;
            _invoiceService.Save(invoice);

            // Update Campaign
            campaign.Status = PCL.Enums.CampaignStatus.Cancelled;
            campaign.CancelledDate = DateTime.UtcNow;
            _campaignService.SaveCampaign(campaign);

            if (Request.QueryString["campaignId"] != null)
            {
                return RedirectToCurrentUmbracoPage("campaignId=" + Request.QueryString["campaignId"]);
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}