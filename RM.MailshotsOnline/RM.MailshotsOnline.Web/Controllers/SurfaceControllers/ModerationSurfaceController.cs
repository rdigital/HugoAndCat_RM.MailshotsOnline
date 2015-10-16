using Glass.Mapper.Umb;
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
        private IUmbracoService _umbracoService;
        private PayPalService _paypalService;
        private const string CompletedFlag = "ModerationResultSaved";
        private const string PaymentFailedFlag = "PaymentFailed";

        public ModerationSurfaceController(IMembershipService membershipService, ILogger logger, ICampaignService campaignService, ISparqQueueService sparqService, IInvoiceService invoiceService, IUmbracoService umbracoService)
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _sparqService = sparqService;
            _invoiceService = invoiceService;
            _paypalService = new PayPalService();
            _umbracoService = umbracoService;
        }

        [ChildActionOnly]
        public ActionResult ShowPrintConfirmationButton(ModerationPage model)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return Complete(model);
            }
            if (TempData[PaymentFailedFlag] != null && (bool)TempData[PaymentFailedFlag] == true)
            {
                return PaymentFailed(model);
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
            campaign.OrderDespatchedDate = DateTime.UtcNow;
            _campaignService.SaveCampaign(campaign);

            Log.Info(this.GetType().Name, "ConfirmPrinted", "Campaign with ID {0} has been printed", campaign.CampaignId);

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

            // Cancel invoice and Void PayPal order
            // Get the invoice
            var campaignInvoices = _invoiceService.GetInvoicesForCampaign(campaign);
            if (campaignInvoices == null || !campaignInvoices.Any())
            {
                Log.Warn(this.GetType().Name, "Reject", "No invoices found for campaign ID {0}", campaign.CampaignId);
                ModelState.AddModelError("ModerationId", "No invoices found for the campaign.");
                return CurrentUmbracoPage();
            }

            var invoice = campaignInvoices.FirstOrDefault(i => i.Status == PCL.Enums.InvoiceStatus.Processing);
            if (invoice == null)
            {
                Log.Warn(this.GetType().Name, "Reject", "No submitted invoices found for campaign ID {0}", campaign.CampaignId);
                ModelState.AddModelError("ModerationId", "No invoices found for the campaign.");
                return CurrentUmbracoPage();
            }

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
                    Log.Exception(this.GetType().Name, "Reject", ex);
                    Log.Error(this.GetType().Name, "Reject", "Unable to cancel PayPal order {0}.", invoice.PaypalOrderId);
                }
            }

            // Update invoice
            invoice.Status = PCL.Enums.InvoiceStatus.Cancelled;
            _invoiceService.Save(invoice);

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

            // Charge paypal
            // Get the invoice
            var campaignInvoices = _invoiceService.GetInvoicesForCampaign(campaign);
            if (campaignInvoices == null || !campaignInvoices.Any())
            {
                Log.Error(this.GetType().Name, "Approve", "No invoices found for campaign ID {0}", campaign.CampaignId);
                ModelState.AddModelError("ModerationId", "Internal invoicing error.");
                return CurrentUmbracoPage();
            }

            var invoice = campaignInvoices.FirstOrDefault(i => i.Status == PCL.Enums.InvoiceStatus.Processing);
            if (invoice == null)
            {
                Log.Error(this.GetType().Name, "Approve", "No invoices in progress found for campaign ID {0}", campaign.CampaignId);
                ModelState.AddModelError("ModerationId", "Internal invoicing error.");
                return CurrentUmbracoPage();
            }

            // Get the PayPal order
            Order order = null;
            try
            {
                order = _paypalService.GetOrder(invoice.PaypalOrderId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Approve", ex);
                Log.Error(this.GetType().Name, "Approve", "Unable to fetch PayPal order {0}.", invoice.PaypalOrderId);
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
                Log.Exception(this.GetType().Name, "Approve", ex);
                Log.Error(this.GetType().Name, "Approve", "Unable to capture payment for order {0}.", order.Id);
            }

            if (result == null)
            {
                invoice.Status = PCL.Enums.InvoiceStatus.Failed;
                invoice.CancelledDate = DateTime.UtcNow;
                _invoiceService.Save(invoice);

                campaign.Status = PCL.Enums.CampaignStatus.PaymentFailed;
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Error capturing payment for PayPal order {2}", Environment.NewLine, DateTime.UtcNow, order.Id);
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Payment failed for campaign.", Environment.NewLine, DateTime.UtcNow);
                _campaignService.SaveCampaign(campaign);

                Log.Error(this.GetType().Name, "Approve", "Payment failed for campaign: Error capturing payment for PayPal order {0}.", order.Id);

                ModelState.AddModelError("ModerationId", "Error getting PayPal order information");
                TempData[PaymentFailedFlag] = true;
                return CurrentUmbracoPage();
            }

            invoice.PaypalCaptureTransactionId = result.Id;

            // Get the Order again and confirm that the payment has worked
            Order updatedOrder = null;
            try
            {
                updatedOrder = _paypalService.GetOrder(invoice.PaypalOrderId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Approve", ex);
                Log.Error(this.GetType().Name, "Approve", "Unable to fetch updated PayPal order {0} after capture attempt.", invoice.PaypalOrderId);
            }

            if (updatedOrder == null || updatedOrder.State != OrderState.completed)
            {
                invoice.Status = PCL.Enums.InvoiceStatus.Failed;
                invoice.CancelledDate = DateTime.UtcNow;
                _invoiceService.Save(invoice);

                var errorMessage = string.Format("Payment failed for campaign: PayPal order {0} is not in the \"Complete\" state..", invoice.PaypalOrderId);
                if (updatedOrder != null)
                {
                    errorMessage += string.Format(" Returned state is: {0}.", updatedOrder.State);
                    if (updatedOrder.ReasonCode.HasValue)
                    {
                        errorMessage += string.Format("  Reason code is: {0}.", updatedOrder.ReasonCode);
                    }
                }

                Log.Error(this.GetType().Name, "Approve", errorMessage);

                campaign.Status = PCL.Enums.CampaignStatus.PaymentFailed;
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - {2}.", Environment.NewLine, DateTime.UtcNow, errorMessage);
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Payment failed for campaign.", Environment.NewLine, DateTime.UtcNow);
                _campaignService.SaveCampaign(campaign);

                ModelState.AddModelError("ModerationId", "Error getting PayPal order information");
                TempData[PaymentFailedFlag] = true;
                return CurrentUmbracoPage();
            }

            invoice.Status = PCL.Enums.InvoiceStatus.Paid;
            invoice.PaidDate = DateTime.UtcNow;
            _invoiceService.Save(invoice);

            var fullInvoice = _invoiceService.GetInvoice(invoice.InvoiceId);

            // Generate invoice PDF
            var moderationPage = _umbracoService.GetItem<ModerationPage>(CurrentPage.Id);
            var xmlAndXsl = new XmlAndXslData()
            {
                XslStylesheet = moderationPage.InvoiceXsl,
                XmlData = fullInvoice.ToXmlString()
            };

            var baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
            var postbackUrl = string.Format("{0}/Umbraco/Api/Orders/InvoicePdfReady/{1}", baseUrl, invoice.InvoiceId);
            _sparqService.SendRenderJob(xmlAndXsl, invoice.InvoiceId.ToString(), "Invoice", postbackUrl);

            campaign.Status = PCL.Enums.CampaignStatus.ReadyForFulfilment;
            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Campaign has been approved.", Environment.NewLine, DateTime.UtcNow);
            _campaignService.SaveCampaign(campaign);

            Log.Info(this.GetType().Name, "Approve", "Campaign with ID {0} has been approved", campaign.CampaignId);

            //TODO: Send the PDF off to print
            var printPostbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/PrintRenderReady/{1}&campaignId={2}", baseUrl, campaign.Mailshot.MailshotId, campaign.CampaignId);
            var ftpPostbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/JobReadyForPrint/{1}", baseUrl, campaign.CampaignId);
            _sparqService.SendRenderAndPrintJob(campaign.Mailshot, printPostbackUrl, ftpPostbackUrl);

            TempData[CompletedFlag] = true;
            return CurrentUmbracoPage();
        }

        private ActionResult Complete(ModerationPage pageModel)
        {
            return PartialView("~/Views/Moderation/Partials/Complete.cshtml", pageModel);
        }

        private ActionResult PaymentFailed(ModerationPage pageModel)
        {
            return PartialView("~/Views/Moderation/Partials/PaymentFailed.cshtml", pageModel);
        }
    }
}