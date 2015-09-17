using HC.RM.Common.PCL.Helpers;
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
using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using RM.MailshotsOnline.Entities.JsonModels;
using Newtonsoft.Json;
using RM.MailshotsOnline.Entities.DataModels;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    [Authorize]
    public class CampaignSurfaceController : BaseSurfaceController
    {
        private readonly ICampaignService _campaignService;
        private readonly IPricingService _pricingService;
        private readonly IInvoiceService _invoiceService;
        private readonly IMailshotsService _mailshotService;
        private readonly IMailshotSettingsService _settingsService;

        public CampaignSurfaceController(
            IMembershipService membershipService, 
            ILogger logger, 
            ICampaignService campaignService, 
            IPricingService pricingService, 
            IInvoiceService invoiceService,
            IMailshotsService mailshotsService,
            IMailshotSettingsService mailshotSettingsService) 
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _pricingService = pricingService;
            _invoiceService = invoiceService;
            _mailshotService = mailshotsService;
            _settingsService = mailshotSettingsService;
        }

        [ChildActionOnly]
        public ActionResult ShowStartDesignButton(CampaignHub model)
        {
            var viewModel = new StartDesignViewModel() { PageModel = model };
            Guid campaignId = Guid.Empty;
            if (Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                viewModel.CampaignId = campaignId;
            }

            return PartialView("~/Views/CampaignHub/Partials/StartDesignButton.cshtml", viewModel);
        }

        public ActionResult StartDesign(StartDesignViewModel viewModel)
        {
            // Fetch the campaign
            ICampaign campaign = null;
            try
            {
                campaign = _campaignService.GetCampaign(viewModel.CampaignId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "StartDesign", ex);
                Log.Error(this.GetType().Name, "StartDesign", "Unable to get campaign");
                ModelState.AddModelError("PageModel", "Unable to get campaign");
                return CurrentUmbracoPage();
            }

            if (campaign.UserId != LoggedInMember.Id)
            {
                Log.Error(this.GetType().Name, "StartDesign", "Unauthorised attempt to get campaign with ID {0}", viewModel.CampaignId);
                ModelState.AddModelError("PageModel", "Not able to modify campaign");
                return CurrentUmbracoPage();
            }

            // Create new Mailshot
            // TODO: Include the ability to choose the format.  For now hard-coding to Layout 2 (Card)
            var defaultContent = _settingsService.GetMailshotDefaultContent(1);
            IFormat format = null;
            ITemplate template = null;
            ITheme theme = null;
            MailshotEditorContent parsedContent = null;
            try
            {
                parsedContent = JsonConvert.DeserializeObject<MailshotEditorContent>(defaultContent.JsonData);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "StartDesign", ex);
                Log.Error(this.GetType().Name, "StartDesign", "Unable to parse the default content for the mailshot");
                ModelState.AddModelError("PageModel", "Default content error");
                return CurrentUmbracoPage();
            }

            format = _settingsService.GetFormatByJsonIndex(parsedContent.FormatId);
            template = _settingsService.GetTemplateByJsonIndex(parsedContent.TemplateId, parsedContent.FormatId);
            theme = _settingsService.GetThemeByJsonIndex(parsedContent.ThemeId);

            if (format == null)
            {
                Log.Error(this.GetType().Name, "StartDesign", "Unable to get default format");
                ModelState.AddModelError("PageModel", "Unable to get default format");
                return CurrentUmbracoPage();
            }

            if (template == null)
            {
                Log.Error(this.GetType().Name, "StartDesign", "Unable to get default template");
                ModelState.AddModelError("PageModel", "Unable to get default template");
                return CurrentUmbracoPage();
            }

            if (theme == null)
            {
                Log.Error(this.GetType().Name, "StartDesign", "Unable to get default theme");
                ModelState.AddModelError("PageModel", "Unable to get default theme");
                return CurrentUmbracoPage();
            }

            // Save the mailshot
            Mailshot mailshotData = new Mailshot();

            mailshotData.Content = new MailshotContent() { Content = defaultContent.JsonData };
            mailshotData.UserId = LoggedInMember.Id;
            mailshotData.UpdatedDate = DateTime.UtcNow;
            mailshotData.FormatId = format.FormatId;
            mailshotData.TemplateId = template.TemplateId;
            mailshotData.ThemeId = theme.ThemeId;
            mailshotData.Name = campaign.Name;
            mailshotData.Draft = true;

            var linkedImages = parsedContent.Elements.Where(e => !string.IsNullOrEmpty(e.Src)).Select(e => e.Src);

            IMailshot savedMailshot = null;

            try
            {
                savedMailshot = _mailshotService.SaveMailshot(mailshotData);
                _mailshotService.UpdateLinkedImages(savedMailshot, linkedImages);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "StartDesign", ex);
                Log.Error(this.GetType().Name, "StartDesign", "Error when attempting to save new mailshot: {0}", ex.Message);
            }

            if (savedMailshot == null)
            {
                ModelState.AddModelError("PageModel", "Unable to create new design.");
                return CurrentUmbracoPage();
            }

            // Assign the mailshot to the campaign and save
            campaign.MailshotId = savedMailshot.MailshotId;
            try
            {
                _campaignService.SaveCampaign(campaign);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "StartDesign", ex);
                ModelState.AddModelError("PageModel", "Unable to update campaign with new design.");
                return CurrentUmbracoPage();
            }

            // Redirect the user to the create-canvas page with the appropriate QS parameters
            return Redirect(string.Format("/create-canvas/?formatId={0}&mailshotId={1}", format.JsonIndex, savedMailshot.MailshotId));
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

            // Generate these properly
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
                Log.Error(this.GetType().Name, "Checkout", "PayPal payment {0} does not include an Approval URL.", paymentResult.Id);
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
                invoice.Status = PCL.Enums.InvoiceStatus.Draft;
                invoice.PaypalApprovalUrl = paymentResult.ApprovalUrl;
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

                // Confirm that the user has entered all their details
                if (!LoggedInMember.AllDetailsEntered)
                {
                    var profileUpdatePageId = (int)CurrentPage.GetProperty("profileUpdatePage").Value;
                    var profileUpdatePageUrl = Umbraco.NiceUrl(profileUpdatePageId);
                    
                    return Redirect(string.Format("{0}?campaignId={1}", profileUpdatePageUrl, campaignId));
                }

                invoice.Status = PCL.Enums.InvoiceStatus.Submitted;
                _invoiceService.Save(invoice);

                return Redirect(paymentResult.ApprovalUrl);
            }
        }
    }
}