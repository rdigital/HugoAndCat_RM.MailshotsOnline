using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Order = HC.RM.Common.PayPal.Models.Order;
using Payment = HC.RM.Common.PayPal.Models.Payment;
using PayPalService = HC.RM.Common.PayPal.Service;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class PaymentCancelledController : GlassController
    {
        private ICampaignService _campaignService;
        private IMembershipService _membershipService;
        private IInvoiceService _invoiceService;

        public PaymentCancelledController(
            IUmbracoService umbracoService,
            ILogger logger,
            ICampaignService campaignService,
            IMembershipService membershipService,
            IInvoiceService invoiceService)
            : base(umbracoService, logger)
        {
            _campaignService = campaignService;
            _membershipService = membershipService;
            _invoiceService = invoiceService;
        }

        [Authorize]
        // GET: PaymentCancelled
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<PaymentCancelled>();
            pageModel.DisplayCampaignErrorMessage = false;
            pageModel.DisplayPaypalErrorMessage = false;

            // Check to see if there's a Campaign in the query string
            Guid campaignId = Guid.Empty;
            if (!Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                _logger.Warn(this.GetType().Name, "Index", "Attempt to access PaymentCancelled page without campaign ID");
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign == null)
            {
                _logger.Warn(this.GetType().Name, "Index", "Attempt to access PaymentCancelled page with invalid campaign ID {0}", campaignId);
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            var loggedInMember = _membershipService.GetCurrentMember();
            if (campaign.UserId != loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Index", "Unauthorised attempt to access PaymentCancelled page with campaign ID {0} which belongs to a different user", campaignId);
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            // Get the invoice
            var campaignInvoices = _invoiceService.GetInvoicesForCampaign(campaign);
            if (campaignInvoices == null || !campaignInvoices.Any())
            {
                _logger.Warn(this.GetType().Name, "Index", "No invoices found for campaign ID {0}", campaignId);
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            string paymentId = Request.QueryString["paymentId"];
            var invoice = campaignInvoices.FirstOrDefault(i => i.Status == PCL.Enums.InvoiceStatus.Submitted && i.PaypalPaymentId == paymentId);
            if (invoice == null)
            {
                _logger.Warn(this.GetType().Name, "Index", "No invoices found for campaign ID {0} with PayPal payment ID {1}", campaignId, paymentId);
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            // Save the details
            invoice.Status = PCL.Enums.InvoiceStatus.Cancelled;
            _invoiceService.Save(invoice);

            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Payment for invoice {2} cancelled.", Environment.NewLine, DateTime.UtcNow, invoice.InvoiceId);
            campaign.Status = PCL.Enums.CampaignStatus.ReadyToCheckout;
            _campaignService.SaveCampaign(campaign);

            return View("~/Views/PaymentCancelled.cshtml", pageModel);
        }

        private ActionResult ReturnView(PaymentCancelled pageModel)
        {
            return View("~/Views/PaymentCancelled.cshtml", pageModel);
        }
    }
}