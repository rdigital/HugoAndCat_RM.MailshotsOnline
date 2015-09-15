using HC.RM.Common.PayPal.Models;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPalService = HC.RM.Common.PayPal.Service;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ModerationSurfaceController : BaseSurfaceController
    {
        private ICampaignService _campaignService;
        private ISparqQueueService _sparqService;
        private IInvoiceService _invoiceService;
        private PayPalService _paypalService;
        private const string CompletedFlag = "ModerationResultSaved";

        public ModerationSurfaceController(IMembershipService membershipService, ILogger logger, ICampaignService campaignService, ISparqQueueService sparqService, IInvoiceService invoiceService)
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _sparqService = sparqService;
            _invoiceService = invoiceService;
            _paypalService = new PayPalService();
        }

        [ChildActionOnly]
        public ActionResult ShowPrintConfirmationButton(ModerationPage model)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return Complete(model);
            }

            Guid moderationId = Guid.Parse(Request.QueryString["moderationId"]);
            var viewModel = new ModerationPrintedViewModel() { PageModel = model, ModerationId = moderationId };
            return PartialView("~/Views/Moderation/Partials/PrintConfirmation.cshtml", viewModel);
        }

        public ActionResult ConfirmPrinted(ModerationPrintedViewModel viewModel)
        {
            ICampaign campaign = null;
            try
            {
                campaign = _campaignService.GetCampaignByModerationId(viewModel.ModerationId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "ConfirmPrinted", ex);
            }

            if (campaign == null)
            {
                Log.Error(this.GetType().Name, "ConfirmPrinted", "Campaign with Moderation ID {0} could not be found.");
                ModelState.AddModelError("ModerationId", "No campaign found with the given ID.");
                return CurrentUmbracoPage();
            }

            campaign.Status = PCL.Enums.CampaignStatus.Fulfilled;
            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Campaign has been printed.", Environment.NewLine, DateTime.UtcNow);
            _campaignService.SaveCampaign(campaign);

            Log.Info(this.GetType().Name, "ConfirmPrinted", "Campaign with ID {0} has been printed", campaign.CampaignId);

            //TODO: Charge paypal
            // Get the invoice
            var campaignInvoices = _invoiceService.GetInvoicesForCampaign(campaign);
            if (campaignInvoices == null || !campaignInvoices.Any())
            {
                Log.Error(this.GetType().Name, "ConfirmPrinted", "No invoices found for campaign ID {0}", campaign.CampaignId);
                ModelState.AddModelError("ModerationId", "Internal invoicing error.");
                return CurrentUmbracoPage();
            }

            var invoice = campaignInvoices.FirstOrDefault(i => i.Status == PCL.Enums.InvoiceStatus.Processing);
            if (invoice == null)
            {
                Log.Error(this.GetType().Name, "ConfirmPrinted", "No invoices in progress found for campaign ID {0}", campaign.CampaignId);
                ModelState.AddModelError("ModerationId", "Internal invoicing error.");
                return CurrentUmbracoPage();
            }

            /*// Get the PayPal payment / order
            Payment payment = null;
            try
            {
                payment = _paypalService.GetPayment(invoice.PaypalPaymentId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "ConfirmPrinted", ex);
                Log.Error(this.GetType().Name, "ConfirmPrinted", "Error getting PayPal payment information for payment ID {0}.", invoice.PaypalPaymentId);
            }

            if (payment == null)
            {
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Error getting PayPal payment information for payment ID {2}.", Environment.NewLine, DateTime.UtcNow, invoice.PaypalPaymentId);
                _campaignService.SaveCampaign(campaign);
                ModelState.AddModelError("ModerationId", "Error getting PayPal payment information.");
                return CurrentUmbracoPage();
            }*/

            // Get the PayPal order
            Order order = null;
            try
            {
                order = _paypalService.GetOrder(invoice.PaypalOrderId);
            }
            catch(Exception ex)
            {
                Log.Exception(this.GetType().Name, "ConfirmPrinted", ex);
                Log.Error(this.GetType().Name, "ConfirmPrinted", "Unable to fetch PayPal order {0}.", invoice.PaypalOrderId);
            }

            if (order == null)
            {
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Error getting PayPal order information for order {2}", Environment.NewLine, DateTime.UtcNow, invoice.PaypalOrderId);
                _campaignService.SaveCampaign(campaign);
                ModelState.AddModelError("ModerationId", "Error getting PayPal order information");
                return CurrentUmbracoPage();
            }

            // Capture the full order amount
            Capture result = null;
            try
            {
                result = _paypalService.CaptureFullAmountForOrder(order);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "ConfirmPrinted", ex);
                Log.Error(this.GetType().Name, "ConfirmPrinted", "Unable to capture payment for order {0}.", order.Id);
            }

            if (result == null)
            {
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Error capturing payment for PayPal order {2}", Environment.NewLine, DateTime.UtcNow, order.Id);
                _campaignService.SaveCampaign(campaign);
                ModelState.AddModelError("ModerationId", "Error getting PayPal order information");
                return CurrentUmbracoPage();
            }

            invoice.Status = PCL.Enums.InvoiceStatus.Paid;
            _invoiceService.Save(invoice);

            TempData[CompletedFlag] = true;
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowRejectionConfirmationForm(ModerationPage model)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return Complete(model);
            }

            Guid moderationId = Guid.Parse(Request.QueryString["moderationId"]);
            var viewModel = new ModerationRejectionViewModel() { PageModel = model, ModerationId = moderationId };
            return PartialView("~/Views/Moderation/Partials/RejectConfirmation.cshtml", viewModel);
        }

        public ActionResult Reject(ModerationRejectionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            ICampaign campaign = null;
            try
            {
                campaign = _campaignService.GetCampaignByModerationId(viewModel.ModerationId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Reject", ex);
            }

            if (campaign == null)
            {
                Log.Error(this.GetType().Name, "Reject", "Campaign with Moderation ID {0} could not be found.");
                ModelState.AddModelError("ModerationId", "No campaign found with the given ID.");
                return CurrentUmbracoPage();
            }

            campaign.Status = PCL.Enums.CampaignStatus.Exception;
            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Campaign has been rejected.", Environment.NewLine, DateTime.UtcNow);
            campaign.SystemNotes += string.Format("{0}Reason message: {1}", Environment.NewLine, viewModel.FeedbackMessage);
            _campaignService.SaveCampaign(campaign);

            Log.Info(this.GetType().Name, "Reject", "Campaign with ID {0} has been rejected.  Reason: {1}", campaign.CampaignId, viewModel.FeedbackMessage);

            TempData[CompletedFlag] = true;
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowApprovalConfirmationButton(ModerationPage model)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return Complete(model);
            }

            Guid moderationId = Guid.Parse(Request.QueryString["moderationId"]);
            var viewModel = new ModerationApprovalViewModel() { PageModel = model, ModerationId = moderationId };
            return PartialView("~/Views/Moderation/Partials/ApprovalConfirmation.cshtml", viewModel);
        }

        public ActionResult Approve(ModerationApprovalViewModel viewModel)
        {
            ICampaign campaign = null;
            try
            {
                campaign = _campaignService.GetCampaignByModerationId(viewModel.ModerationId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Approve", ex);
            }

            if (campaign == null)
            {
                Log.Error(this.GetType().Name, "Approve", "Campaign with Moderation ID {0} could not be found.");
                ModelState.AddModelError("ModerationId", "No campaign found with the given ID.");
                return CurrentUmbracoPage();
            }

            campaign.Status = PCL.Enums.CampaignStatus.ReadyForFulfilment;
            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Campaign has been approved.", Environment.NewLine, DateTime.UtcNow);
            _campaignService.SaveCampaign(campaign);

            Log.Info(this.GetType().Name, "Approve", "Campaign with ID {0} has been approved", campaign.CampaignId);

            //TODO: Send the PDF off to print
            var baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
            var postbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/JobReadyForPrint/{1}&campaignId={2}", baseUrl, campaign.Mailshot.MailshotId, campaign.CampaignId);
            _sparqService.SendRenderAndPrintJob(campaign.Mailshot, postbackUrl);

            TempData[CompletedFlag] = true;
            return CurrentUmbracoPage();
        }

        private ActionResult Complete(ModerationPage pageModel)
        {
            return PartialView("~/Views/Moderation/Partials/Complete.cshtml", pageModel);
        }
    }
}