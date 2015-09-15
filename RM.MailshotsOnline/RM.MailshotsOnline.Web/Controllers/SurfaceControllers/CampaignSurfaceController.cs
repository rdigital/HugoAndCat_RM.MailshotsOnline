﻿using HC.RM.Common.PCL.Helpers;
using HC.RM.Common.PayPal;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayPalService = HC.RM.Common.PayPal.Service;
using RM.MailshotsOnline.Data.Helpers;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    [Authorize]
    public class CampaignSurfaceController : BaseSurfaceController
    {
        private readonly ICampaignService _campaignService;
        private readonly IPricingService _pricingService;
        private readonly IInvoiceService _invoiceService;

        public CampaignSurfaceController(IMembershipService membershipService, ILogger logger, ICampaignService campaignService, IPricingService pricingService, IInvoiceService invoiceService) 
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _pricingService = pricingService;
            _invoiceService = invoiceService;
        }

        [ChildActionOnly]
        public ActionResult ShowCheckoutButton(CampaignHub model)
        {
            var viewModel = new CampaignHubCheckoutViewModel()
            {
                PageModel = model
            };

            return PartialView("~/Views/CampaignHub/Partials/CheckoutButton.cshtml", viewModel);
        }

        public ActionResult Checkout(CampaignHubCheckoutViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Campaign validation (logged in user owns campaign, it's in the right status and all the components are approved)
            // Parent control should stop this from displaying, but worth checking just in case
            Guid campaignId = Guid.Empty;
            ICampaign campaign = null;
            if (Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                campaign = _campaignService.GetCampaign(campaignId);
            }

            if (campaign == null)
            {
                ModelState.AddModelError("PageModel", "Incorrect campaign ID");
                return CurrentUmbracoPage();
            }

            var thisPageUrl = string.Format("{0}?campaignId={1}", CurrentPage.Url, campaignId);

            // Confirm that the user has entered all their details
            if (!LoggedInMember.AllDetailsEntered)
            {
                // TODO: Create the interstitial page for this
                var profileUpdatePageId = (int)CurrentPage.GetProperty("profileUpdatePage").Value;
                var profileUpdatePageUrl = Umbraco.NiceUrl(profileUpdatePageId);

                return Redirect(string.Format("{0}?returnUrl={1}", profileUpdatePageUrl, HttpUtility.UrlEncode(thisPageUrl)));
            }

            // Logged in owner owns campaign?
            if (campaign.UserId != LoggedInMember.Id)
            {
                Log.Error(this.GetType().Name, "Checkout", "Unauthorised attempt to checkout a campaign that doesn't belong to the logged-in user.");
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No access to specified campaign.");
            }

            // Campaign is in correct state?
            if (campaign.Status != PCL.Enums.CampaignStatus.ReadyToCheckout)
            {
                Log.Error(this.GetType().Name, "Checkout", "Attempt to check out campaign that is not in the correct status.");
                ModelState.AddModelError("PageModel", "Your campaign must be finalised and approved before you attempt to check out.");
                return CurrentUmbracoPage();
            }

            // Create a PayPal payment
            IInvoice invoice = null;
            try
            {
                invoice = _invoiceService.CreateInvoiceForCampaign(campaign);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Checkout", ex);
                Log.Error(this.GetType().Name, "Checkout", "Error with checkout for campaign {0}.", campaignId);
            }

            if (invoice == null)
            {
                ModelState.AddModelError("PageModel", "There was an error generating the invoice for your campaign.");
                return CurrentUmbracoPage();
            }

            var baseUrl = Request.Url.ToString();
            var token = "?";
            if (baseUrl.Contains("?"))
            {
                token = "&";
            }

            // TODO: Generate these properly
            var returnPageId = (int)CurrentPage.GetProperty("paymentConfirmationPage").Value;
            var returnPageUrl = Umbraco.NiceUrlWithDomain(returnPageId);
            var returnUrl = string.Format("{0}?campaignId={1}", returnPageUrl, campaignId);

            var cancelPageId = (int)CurrentPage.GetProperty("paymentCancelledPage").Value;
            var cancelPageUrl = Umbraco.NiceUrlWithDomain(cancelPageId);
            var cancelUrl = string.Format("{0}?campaignId={1}&invoiceId={2}", cancelPageUrl, campaignId, invoice.InvoiceId);


            // Generate PayPal payment
            var paypalService = new PayPalService();

            var payer = new HC.RM.Common.PayPal.Models.Payer(HC.RM.Common.PayPal.Models.PayerPaymentMethod.paypal);
            var transaction = new HC.RM.Common.PayPal.Models.Transaction(ConfigHelper.CurrencyCode, invoice.Total, invoice.SubTotal, invoice.TotalTax, invoice.InvoiceId.ToString("D"));

            var paymentRequest = new HC.RM.Common.PayPal.Models.Payment(
                HC.RM.Common.PayPal.Models.PaymentIntent.order,
                payer,
                transaction,
                returnUrl,
                cancelUrl);

            HC.RM.Common.PayPal.Models.Payment paymentResult = null;
            try
            {
                paymentResult = paypalService.CreatePayment(paymentRequest);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Checkout", ex);
                Log.Error(this.GetType().Name, "Checkout", "Error generating PayPal payment for campaign {0}.", campaignId);
            }

            // Either redirect the user to the PayPal auth screen or display error message
            bool readyForPaypal = true;
            if (paymentResult == null)
            {
                readyForPaypal = false;
            }
            else if (string.IsNullOrEmpty(paymentResult.ApprovalUrl))
            {
                readyForPaypal = false;
            }

            if (!readyForPaypal)
            {
                ModelState.AddModelError("PageModel", "There was an error passing the payment through to PayPal.");
                invoice.Status = PCL.Enums.InvoiceStatus.Cancelled;
                _invoiceService.Save(invoice);
                return CurrentUmbracoPage();
            }
            else
            {
                // Save results from PayPal then redirect the user
                invoice.PaypalPaymentId = paymentResult.Id;
                invoice.Status = PCL.Enums.InvoiceStatus.Submitted;
                var transactionResult = paymentResult.Transactions.FirstOrDefault();
                if (transactionResult != null)
                {
                    var order = transactionResult.RelatedResources.FirstOrDefault(r => r is HC.RM.Common.PayPal.Models.Order);
                    if (order != null)
                    {
                        invoice.PaypalOrderId = order.Id;
                    }
                }
                _invoiceService.Save(invoice);

                return Redirect(paymentResult.ApprovalUrl);
            }
        }
    }
}