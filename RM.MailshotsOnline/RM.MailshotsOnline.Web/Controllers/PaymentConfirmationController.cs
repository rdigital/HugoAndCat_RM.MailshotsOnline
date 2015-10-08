using Glass.Mapper.Umb;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Order = HC.RM.Common.PayPal.Models.Order;
using Payment = HC.RM.Common.PayPal.Models.Payment;
using PplAddress = HC.RM.Common.PayPal.Models.Address;
using PayPalService = HC.RM.Common.PayPal.Service;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class PaymentConfirmationController : GlassController
    {
        private readonly ICampaignService _campaignService;
        private readonly IMembershipService _membershipService;
        private readonly IPricingService _pricingService;
        private readonly IInvoiceService _invoiceService;
        private readonly IEmailService _emailService;
        private readonly ISparqQueueService _sparqService;
        private readonly IMailshotsService _mailshotService;

        public PaymentConfirmationController(
            IUmbracoService umbracoService, 
            ILogger logger, 
            ICampaignService campaignService, 
            IMembershipService membershipService, 
            IPricingService pricingService,
            IInvoiceService invoiceService,
            IEmailService emailService,
            ISparqQueueService sparqService,
            IMailshotsService mailshotService)
            : base(umbracoService, logger)
        {
            _campaignService = campaignService;
            _membershipService = membershipService;
            _pricingService = pricingService;
            _invoiceService = invoiceService;
            _emailService = emailService;
            _sparqService = sparqService;
            _mailshotService = mailshotService;
        }

        // GET: PaymentConfirmation
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<PaymentConfirmation>();
            pageModel.DisplayCampaignErrorMessage = false;
            pageModel.DisplayPaypalErrorMessage = false;

            // Check to see if there's a Campaign in the query string
            Guid campaignId = Guid.Empty;
            if (!Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                _logger.Warn(this.GetType().Name, "Index", "Attempt to access PaymentConfirmation page without campaign ID");
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign == null)
            {
                _logger.Warn(this.GetType().Name, "Index", "Attempt to access PaymentConfirmation page with invalid campaign ID {0}", campaignId);
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            var loggedInMember = _membershipService.GetCurrentMember();
            if (campaign.UserId != loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Index", "Unauthorised attempt to access PaymentConfirmation page with campaign ID {0} which belongs to a different user", campaignId);
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

            // Execute PayPal payment and capture order number
            string payerId = Request.QueryString["PayerID"];
            var paypalService = new PayPalService();

            Payment payment = null;
            try
            {
                payment = paypalService.GetPayment(paymentId);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "Index", ex);
                _logger.Error(this.GetType().Name, "Index", "Error getting PayPal payment information for payment ID {0}.", paymentId);
            }

            if (payment == null)
            {
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Error getting PayPal payment information for payment ID {2}.", Environment.NewLine, DateTime.UtcNow, paymentId);
                _campaignService.SaveCampaign(campaign);
                pageModel.DisplayPaypalErrorMessage = true;
                return ReturnView(pageModel);
            }

            Payment result = null;
            try
            {
                result = paypalService.ExecutePayment(payerId, payment);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "Index", ex);
                _logger.Error(this.GetType().Name, "Index", "Error executing PayPal payment with ID {0}.", paymentId);
            }

            if (result == null)
            {
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Error executing PayPal payment with ID {2}.", Environment.NewLine, DateTime.UtcNow, paymentId);
                _campaignService.SaveCampaign(campaign);
                pageModel.DisplayPaypalErrorMessage = true;
                return ReturnView(pageModel);
            }

            // Get the order number and update the invoice
            var transaction = result.Transactions.FirstOrDefault();
            if (transaction == null)
            {
                _logger.Error(this.GetType().Name, "Index", "No transactions found for PayPal payment ID {0}.", paymentId);
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - No transactions found for PayPal payment ID {2}.", Environment.NewLine, DateTime.UtcNow, paymentId);
                _campaignService.SaveCampaign(campaign);
                pageModel.DisplayPaypalErrorMessage = true;
                return ReturnView(pageModel);
            }

            Order order = transaction.RelatedResources.FirstOrDefault(rr => rr is Order) as Order;
            if (order == null)
            {
                _logger.Error(this.GetType().Name, "Index", "No order found in transaction for PayPal payment ID {0}.", paymentId);
                campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - No order found in transaction for PayPal payment ID {2}.", Environment.NewLine, DateTime.UtcNow, paymentId);
                _campaignService.SaveCampaign(campaign);
                pageModel.DisplayPaypalErrorMessage = true;
                return ReturnView(pageModel);
            }

            // Save the details
            invoice.PaypalOrderId = order.Id;
            invoice.Status = PCL.Enums.InvoiceStatus.Processing;
            if (payment.Payer != null)
            {
                PplAddress payerAddress = payment.Payer.BillingAddress;
                if (payerAddress == null)
                {
                    payerAddress = payment.Payer.ShippingAddress;
                }

                if (payerAddress != null)
                {
                    invoice.BillingAddress = new Address()
                    {
                        Address1 = payerAddress.Line1,
                        Address2 = payerAddress.Line2,
                        Postcode = payerAddress.PostalCode,
                        City = payerAddress.City,
                        Country = payerAddress.CountryCode,
                        FirstName = payment.Payer.FirstName,
                        LastName = payment.Payer.LastName,
                        Title = payment.Payer.Salutation
                    };
                }

                invoice.BillingEmail = payment.Payer.Email;
            }
            _invoiceService.Save(invoice);

            campaign.Status = PCL.Enums.CampaignStatus.PendingModeration;
            campaign.OrderPlacedDate = DateTime.UtcNow;
            campaign.ModerationId = Guid.NewGuid();
            _campaignService.SaveCampaign(campaign);

            // Send email to user confirming purchase
            var recipients = new List<string>() { loggedInMember.EmailAddress };
            var sender = new System.Net.Mail.MailAddress(ConfigHelper.SystemEmailAddress);
            _emailService.SendEmail(
                recipients, 
                pageModel.ConfirmationEmailSubject,
                pageModel.ConfirmationEmailToUser,
                System.Net.Mail.MailPriority.Normal,
                sender);

            // Generate preview PDF and send to Royal Mail for approval
            var fullMailshot = _mailshotService.GetMailshot(campaign.MailshotId.Value);
            fullMailshot.ProofPdfOrderNumber = Guid.NewGuid();
            _mailshotService.SaveMailshot(fullMailshot);
            var baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
            var postbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/SendProofForApproval/{1}", baseUrl, campaign.CampaignId);
            _sparqService.SendRenderJob(fullMailshot, postbackUrl);

            return View("~/Views/PaymentConfirmation.cshtml", pageModel);
        }

        private ActionResult ReturnView(PaymentConfirmation pageModel)
        {
            return View("~/Views/PaymentConfirmation.cshtml", pageModel);
        }
    }
}